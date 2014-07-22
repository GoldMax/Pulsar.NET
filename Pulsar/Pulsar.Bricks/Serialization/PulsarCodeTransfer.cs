using System;
using Microsoft.CSharp;
using System.Collections.Generic;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;



namespace Pulsar.Serialization
{
 /// <summary>
 /// Вспомогательный класс для переноса кода.
 /// </summary>
 public static class PulsarCodeTransfer
 {
  private static OpCode[] OneByteOpCodes = new OpCode[0x100];
  private static OpCode[] TwoByteOpCodes = new OpCode[0x100];

  //-------------------------------------------------------------------------------------
  #region << Constructors >>
  /// <summary>
  /// Конструктор по умолчанию.
  /// </summary>
  static PulsarCodeTransfer()
  {
   foreach(FieldInfo fi in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
   {
    OpCode opCode = (OpCode)fi.GetValue(null);
    UInt16 value = (UInt16)opCode.Value;
    if(value < 0x100)
     OneByteOpCodes[value] = opCode;
    else if((value & 0xff00) == 0xfe00)
     TwoByteOpCodes[value & 0xff] = opCode;
   }
  }
  #endregion << Constructors >>
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает текст враппера класса с динамическим методом.
  /// </summary>
  /// <param name="type">Тип класса, для которого создается текст враппера.</param>
  /// <param name="dynMethodBody">Текст динамического метода.</param>
  /// <returns></returns>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
  public static string GetClassWrapperText(Type type, string dynMethodBody)
  {
   string name = type.Name;
   if(name.Contains("`"))
    name = name.Substring(0, type.Name.IndexOf('`'));
   CodeTypeDeclaration ctd = new CodeTypeDeclaration(name);
   if(type.BaseType != typeof(object) && type.BaseType != typeof(ValueType))
    ctd.BaseTypes.Add(type.BaseType);
   foreach(Type t in type.GetInterfaces())
    ctd.BaseTypes.Add(t);
   ctd.IsClass = type.IsClass;
   ctd.IsEnum = type.IsEnum;
   ctd.IsInterface = type.IsInterface;
   ctd.TypeAttributes = type.Attributes;
   foreach(Type t in type.GetGenericArguments())
    ctd.TypeParameters.Add(t.Name);

   BindingFlags bf = BindingFlags.Instance | BindingFlags.Static|
                     BindingFlags.NonPublic | BindingFlags.Public |
                     BindingFlags.DeclaredOnly;

   foreach(MemberInfo mi in type.GetMembers(bf))
    switch(mi.MemberType)
    {
     case MemberTypes.Field:
     {
      if(((FieldInfo)mi).Name.Contains("<"))
       break;
      CodeMemberField ctm = new CodeMemberField();
      if(((FieldInfo)mi).IsPrivate)
       ctm.Attributes = (ctm.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Private;
      if(((FieldInfo)mi).IsPublic)
       ctm.Attributes = (ctm.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
      if(((FieldInfo)mi).IsStatic)
       ctm.Attributes |= MemberAttributes.Static;

      ctm.Type = new CodeTypeReference(((FieldInfo)mi).FieldType);
      ctm.InitExpression = new CodeDefaultValueExpression(ctm.Type);
      ctm.Name = mi.Name;
      ctd.Members.Add(ctm);
     }
     break;
     case MemberTypes.Constructor:
     {
      CodeConstructor cc = new CodeConstructor();
      if(((ConstructorInfo)mi).IsPrivate)
       cc.Attributes = (cc.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Private;
      if(((ConstructorInfo)mi).IsPublic)
       cc.Attributes = (cc.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
      if(((ConstructorInfo)mi).IsStatic)
       cc.Attributes |= MemberAttributes.Static;

      cc.Name = ((ConstructorInfo)mi).Name;
      foreach(ParameterInfo i in ((ConstructorInfo)mi).GetParameters())
       cc.Parameters.Add(new CodeParameterDeclarationExpression(i.ParameterType, i.Name));
      ctd.Members.Add(cc);
     }
     break;
     case MemberTypes.Method:
     {
      if(((MethodInfo)mi).IsSpecialName)
       break;
      CodeMemberMethod cc = new CodeMemberMethod();
      if(((MethodInfo)mi).Name.Contains("."))
       cc.Attributes = (cc.Attributes & ~MemberAttributes.AccessMask);
      else if(((MethodInfo)mi).IsPrivate)
       cc.Attributes = (cc.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Private;
      else if(((MethodInfo)mi).IsPublic)
       cc.Attributes = (cc.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
      if(((MethodInfo)mi).IsStatic)
       cc.Attributes |= MemberAttributes.Static;
      cc.Name = ((MethodInfo)mi).Name;
      foreach(ParameterInfo i in ((MethodInfo)mi).GetParameters())
      {
       CodeParameterDeclarationExpression par = new CodeParameterDeclarationExpression(i.ParameterType, i.Name);
       if(i.IsOut)
        par.Direction = FieldDirection.Out;
       if(i.IsRetval)
        par.Direction = FieldDirection.Ref;
       if(i.IsIn)
        par.Direction = FieldDirection.In;
       if(i.ParameterType.IsByRef)
       {
        par.Type = new CodeTypeReference(i.ParameterType.FullName.Replace("&", ""));
        cc.Statements.Add(new CodeSnippetExpression(
         String.Format("{0} = default({1})", i.Name, par.Type.BaseType)));
       }
       cc.Parameters.Add(par);
      }
      foreach(Type p in ((MethodInfo)mi).GetGenericArguments())
       cc.TypeParameters.Add(p.Name);
      cc.ReturnType = new CodeTypeReference(((MethodInfo)mi).ReturnType);
      if(((MethodInfo)mi).ReturnType != typeof(void))
       cc.Statements.Add(new CodeMethodReturnStatement(new CodeDefaultValueExpression(cc.ReturnType)));
      ctd.Members.Add(cc);
     }
     break;
     case MemberTypes.Property:
     {
      #region ...
      CodeMemberProperty cc = new CodeMemberProperty();
      cc.Name = ((PropertyInfo)mi).Name;
      foreach(ParameterInfo i in ((PropertyInfo)mi).GetIndexParameters())
       cc.Parameters.Add(new CodeParameterDeclarationExpression(i.ParameterType, i.Name));
      cc.Type = new CodeTypeReference(((PropertyInfo)mi).PropertyType);
      cc.HasGet = ((PropertyInfo)mi).CanRead;
      cc.HasSet = ((PropertyInfo)mi).CanWrite;
      cc.GetStatements.Add(new CodeMethodReturnStatement(new CodeDefaultValueExpression(cc.Type)));

      MethodInfo[] mis = ((PropertyInfo)mi).GetAccessors();
      if(((PropertyInfo)mi).Name.Contains("."))
       cc.Attributes = (cc.Attributes & ~MemberAttributes.AccessMask);
      else if(mis.Length > 0)
       cc.Attributes = (cc.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Public;
      else
       cc.Attributes = (cc.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Private;
      ctd.Members.Add(cc);
      #endregion ...
     }
     break;

    }

   CodeMemberMethod dm = new CodeMemberMethod();
   dm.Attributes = (dm.Attributes & ~MemberAttributes.AccessMask) | MemberAttributes.Private;
   dm.Name = "DynamicMethod";
   dm.ReturnType = new CodeTypeReference(typeof(object));
   if(String.IsNullOrWhiteSpace(dynMethodBody))
    dm.Statements.Add(new CodeSnippetStatement("\r\n  //Dynamic Method Body\r\n\r\n\r\n   return null;"));
   dm.Statements.Add(new CodeSnippetStatement(dynMethodBody));
   ctd.Members.Add(dm);

   string s;
   using(CSharpCodeProvider cp = new CSharpCodeProvider())
    using(System.IO.TextWriter tw = new System.IO.StringWriter())
    {
     CodeGeneratorOptions cgo = new CodeGeneratorOptions();
     cgo.BracingStyle = "C";
     cgo.IndentString = " ";
     cgo.BlankLinesBetweenMembers = false;
     cp.GenerateCodeFromType(ctd, tw, cgo);
     s = tw.ToString();
    }
   s = s.Insert(0, "using System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\n" +
                   "\r\nusing Pulsar;\r\nusing Pulsar.Shared;\r\n");
   s = s.Replace(" {\r\n", " { ");
   s = s.Replace("\r\n {", " {");
   s = s.Replace("\r\n  {", " {");
   s = s.Replace("\r\n   {", " {");
   s = s.Replace("\r\n }", " }");
   s = s.Replace("\r\n  }", " }");
   s = s.Replace("\r\n   }", " }");
   s = s.Replace("  ", " ");
   s = s.Replace("  ", " ");
   s = s.Replace("DynamicMethod() {", "DynamicMethod()\r\n {");
   s = s.Replace("\r\n set {", "set {");
   s = s.Replace("\r\n return ", " return ");
   return s;
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Возвращает кодовый запрос для выполнения на Пульсаре
  /// </summary>
  /// <param name="type">Тип класса, для которого создается запрос.</param>
  /// <param name="typeWrapperCode">Текст враппера класса, для которого создается запрос.</param>
  /// <returns>Пара: объект кодового запроса и список ошибок.</returns>
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands")]
  public static ValuesPair<CodeQuery, List<String>> GetCodeQuery(Type type, string typeWrapperCode)
  {
   CSharpCodeProvider cscp = null;

   try
   {
    cscp = new CSharpCodeProvider();
    ValuesPair<CodeQuery, List<String>> res = 
    new ValuesPair<CodeQuery, List<string>>(null, new List<string>());
    CodeQuery query = new CodeQuery();
    query.CodeText = typeWrapperCode;
    query.ObjectType = type.FullName;

    CompilerParameters cp = new CompilerParameters();
    cp.GenerateInMemory = true;
    cp.TreatWarningsAsErrors = false;
    foreach(Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
     if(asm.IsDynamic == false)
      cp.ReferencedAssemblies.Add(asm.Location);
    CompilerResults cr = cscp.CompileAssemblyFromSource(cp, typeWrapperCode);
    if(cr.Errors.Count > 0)
    {
     foreach(CompilerError err in cr.Errors)
      res.Value2.Add(String.Format("[{0},{1}] {2} {3}: {4}",
       err.Line, err.Column, err.IsWarning ? "Warning" : "Error", err.ErrorNumber, err.ErrorText));
    }
    if(cr.Errors.HasErrors)
     return res;

    MethodInfo mi = cr.CompiledAssembly.GetType(type.Name).GetMethod("DynamicMethod",
                                                                     BindingFlags.NonPublic | BindingFlags.Instance);
    MethodBody mb = mi.GetMethodBody();
    query.MaxStackSize = mb.MaxStackSize;
    query.Body = mb.GetILAsByteArray();
    query.Schema = Discover(query.Body, mi, type);

    if(mb.LocalVariables.Count > 0)
    {
     query.LocalVariables = new List<KeyValuePair<Type, bool>>(mb.LocalVariables.Count);
     foreach(LocalVariableInfo lvi in mb.LocalVariables)
      query.LocalVariables.Add(new KeyValuePair<Type, bool>(lvi.LocalType, lvi.IsPinned));
    }
    res.Value1 = query;

    return res;
   }
   finally
   {
    if(cscp != null)
     cscp.Dispose();
   }
  }
  //-------------------------------------------------------------------------------------
  /// <summary>
  /// Выполняет кодовый запрос на объекте.
  /// </summary>
  /// <param name="dstObject">Объект, на котором будет выполнен запрос.</param>
  /// <param name="query">Выполняемый кодовый запрос.</param>
  public static object ExecCodeQuery(object dstObject, CodeQuery query)
  {
   try
   {
    DynamicMethod dm = new DynamicMethod("DynMethod", MethodAttributes.Public | MethodAttributes.Static,
    CallingConventions.Standard, typeof(object), new Type[] { dstObject.GetType() }, dstObject.GetType(), true);
    //DynamicMethod dm = new DynamicMethod("DynMethod", null, null, dstObject.GetType(), true);
    DynamicILInfo dii = dm.GetDynamicILInfo();

    SignatureHelper sig = SignatureHelper.GetLocalVarSigHelper();
    if(query.LocalVariables != null)
     foreach(KeyValuePair<Type, bool> i in query.LocalVariables)
      sig.AddArgument(i.Key, i.Value);
    dii.SetLocalSignature(sig.GetSignature());

    Customize(query.Body, dii, query.Schema);

    dii.SetCode(query.Body, query.MaxStackSize);

    DynMethod deleg = (DynMethod)dm.CreateDelegate(typeof(DynMethod), dstObject);
    return deleg();

    //dm.Invoke(dstObject, new object[] { dstObject });
   }
   catch
   {
    throw;
   }
  }
  //-------------------------------------------------------------------------------------
  internal delegate object DynMethod ();
  //-------------------------------------------------------------------------------------
  internal static Dictionary<int, DiscoverMemberInfo> Discover(byte[] body, MethodInfo mi, Type dstType)
  {
   Dictionary<int, DiscoverMemberInfo> res = new Dictionary<int, DiscoverMemberInfo>();

   ArrayReader ar = new ArrayReader(body);
   while(ar.CanRead)
   {
    int offset = ar.Position;
    OpCode opCode = OpCodes.Nop;
    //int token = 0;

    Byte code = ar.ReadByte();
    if(code != 0xFE)
     opCode = OneByteOpCodes[code];
    else
     opCode = TwoByteOpCodes[ar.ReadByte()];

    switch(opCode.OperandType)
    {
     #region ...
     case OperandType.InlineNone: break;
     //The operand is an 8-bit integer branch target.
     case OperandType.ShortInlineBrTarget: ar.ReadSByte(); break;
     //The operand is a 32-bit integer branch target.
     case OperandType.InlineBrTarget: ar.ReadInt32(); break;
     //The operand is an 8-bit integer: 001F  ldc.i4.s, FE12  unaligned.
     case OperandType.ShortInlineI: ar.ReadByte(); break;
     //The operand is a 32-bit integer.
     case OperandType.InlineI: ar.ReadInt32(); break;
     //The operand is a 64-bit integer.
     case OperandType.InlineI8: ar.ReadInt64(); break;
     //The operand is a 32-bit IEEE floating point number.
     case OperandType.ShortInlineR: ar.ReadSingle(); break;
     //The operand is a 64-bit IEEE floating point number.
     case OperandType.InlineR: ar.ReadDouble(); break;
     //The operand is an 8-bit integer containing the ordinal of a local variable or an argument
     case OperandType.ShortInlineVar: ar.ReadByte(); break;
     //The operand is 16-bit integer containing the ordinal of a local variable or an argument.
     case OperandType.InlineVar: ar.ReadUInt16(); break;
     #endregion ...

     //The operand is a 32-bit metadata string token.
     case OperandType.InlineString:
     {
      #region ...
      int token = ar.ReadInt32();
      if(res.ContainsKey(token))
       break;
      string s = mi.Module.ResolveString(token);
      res.Add(token, new DiscoverMemberInfo(s)); 
      #endregion ...
     }
     break;

     //The operand is a 32-bit metadata signature token.
     case OperandType.InlineSig:
     {
      #region ...
      throw new Exception("НАДО ПРОВЕРИТЬ InlineSig !!!");
      //int token = ar.ReadInt32();
      //if(res.ContainsKey(token))
      // break;

      #endregion ...
     }
     //break;

     //The operand is a 32-bit metadata token.
     case OperandType.InlineMethod:
     {
      #region ...
      int token = ar.ReadInt32();
      if(res.ContainsKey(token))
       break;
      MethodBase mb = mi.Module.ResolveMethod(token);
      if(mb == null)
       throw new PulsarException("Не удалось определить MethodBase для операнда {0}:{1}", opCode, opCode.OperandType);

      DiscoverMemberInfo dmi = new DiscoverMemberInfo();
      dmi.Name = mb.Name;
      dmi.GenericArguments = (mb.IsConstructor) ? null : mb.GetGenericArguments();
      //dmi.DeclaringTypeGenericArguments = (mb.DeclaringType ?? typeof(Type)).GetGenericArguments();
      dmi.ParametersTypes = dmi.GetParametesTypes(mb.GetParameters());
      if(mi.DeclaringType.Equals(mb.DeclaringType))
       dmi.DeclaringType = dstType;
      else
       dmi.DeclaringType = mb.DeclaringType;
      res.Add(token, dmi); 
      #endregion ...
     }
     break;

     //The operand is a 32-bit metadata token.
     case OperandType.InlineField:
     {
      #region ...
      int token = ar.ReadInt32();
      if(res.ContainsKey(token))
       break;
      FieldInfo fi = mi.Module.ResolveField(token);
      if(fi == null)
       throw new PulsarException("Не удалось определить FieldInfo для операнда {0}:{1}", opCode, opCode.OperandType);

      DiscoverMemberInfo dmi = new DiscoverMemberInfo();
      dmi.Name = fi.Name;
      if(mi.DeclaringType.Equals(fi.DeclaringType))
       dmi.DeclaringType = dstType;
      else
       dmi.DeclaringType = fi.DeclaringType;
      res.Add(token, dmi); 
      #endregion ...
     }
     break;

     //The operand is a 32-bit metadata token.
     case OperandType.InlineType:
     {
      #region ...
      int token = ar.ReadInt32();
      if(res.ContainsKey(token))
       break;
      Type t = mi.Module.ResolveType(token, null, null);
      if(t == null)
       throw new PulsarException("Не удалось определить Type для операнда {0}:{1}", opCode, opCode.OperandType);

      DiscoverMemberInfo dmi = new DiscoverMemberInfo();
      dmi.Name = t.FullName;
      dmi.DeclaringType = t;
      dmi.GenericArguments = t.GetGenericArguments();
      res.Add(token, dmi); 
      #endregion ...
     }
     break;

     //The operand is a FieldRef, MethodRef, or TypeRef token.
     case OperandType.InlineTok:
     {
      #region ...
      throw new Exception("НАДО ПРОВЕРИТЬ InlineTok !!!");
      //int token = ar.ReadInt32();
      //if(res.ContainsKey(token))
      // break;
      //     MemberInfo mi = inlineTokInstruction.Member;
      //int token = 0;
      //if(mi.MemberType == MemberTypes.TypeInfo || mi.MemberType == MemberTypes.NestedType)
      //{
      // Type type = mi as Type;
      // token = ilInfo.GetTokenFor(type.TypeHandle);
      //}
      //else if(mi.MemberType == MemberTypes.Method || mi.MemberType == MemberTypes.Constructor)
      //{
      // MethodBase m = mi as MethodBase;
      // token = ilInfo.GetTokenFor(m.MethodHandle, m.DeclaringType.TypeHandle);
      //}
      //else if(mi.MemberType == MemberTypes.Field)
      //{
      // FieldInfo f = mi as FieldInfo;
      // //CLR BUG: token = ilInfo.GetTokenFor(f.FieldHandle, f.DeclaringType.TypeHandle);
      // token = ilInfo.GetTokenFor(f.FieldHandle);
      //}

      //OverwriteInt32(token,
      //   inlineTokInstruction.Offset + inlineTokInstruction.OpCode.Size);

      #endregion ...
     }
     // break;

     //The operand is the 32-bit integer argument to a switch instruction.
     case OperandType.InlineSwitch:
     {
      #region ...
      throw new Exception("НАДО ПРОВЕРИТЬ InlineSwitch !!!");
      //Int32 cases = ar.ReadInt32();
      //Int32[] deltas = new Int32[cases];
      //for(Int32 i = 0; i < cases; i++)
      // deltas[i] = ar.ReadInt32();

      #endregion ...
     }
     //break;
     default:
     throw new BadImageFormatException("Unexpected OperandType " + opCode.OperandType);
    }
   }
   return res;
  }
  //-------------------------------------------------------------------------------------
  internal static void Customize(byte[] body, DynamicILInfo ilInfo, Dictionary<int, DiscoverMemberInfo> schema)
  {
   BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.GetField |  BindingFlags.Instance | 
                                       BindingFlags.Static;
   ArrayReader ar = new ArrayReader(body);
   while(ar.CanRead)
   {
    int offset = ar.Position;
    OpCode opCode = OpCodes.Nop;
    //int token = 0;

    Byte code = ar.ReadByte();
    if(code != 0xFE)
     opCode = OneByteOpCodes[code];
    else
     opCode = TwoByteOpCodes[ar.ReadByte()];

    switch(opCode.OperandType)
    {
     #region ...
     case OperandType.InlineNone: break;
     //The operand is an 8-bit integer branch target.
     case OperandType.ShortInlineBrTarget: ar.ReadSByte(); break;
     //The operand is a 32-bit integer branch target.
     case OperandType.InlineBrTarget: ar.ReadInt32(); break;
     //The operand is an 8-bit integer: 001F  ldc.i4.s, FE12  unaligned.
     case OperandType.ShortInlineI: ar.ReadByte(); break;
     //The operand is a 32-bit integer.
     case OperandType.InlineI: ar.ReadInt32(); break;
     //The operand is a 64-bit integer.
     case OperandType.InlineI8: ar.ReadInt64(); break;
     //The operand is a 32-bit IEEE floating point number.
     case OperandType.ShortInlineR: ar.ReadSingle(); break;
     //The operand is a 64-bit IEEE floating point number.
     case OperandType.InlineR: ar.ReadDouble(); break;
     //The operand is an 8-bit integer containing the ordinal of a local variable or an argument
     case OperandType.ShortInlineVar: ar.ReadByte(); break;
     //The operand is 16-bit integer containing the ordinal of a local variable or an argument.
     case OperandType.InlineVar: ar.ReadUInt16(); break;
     #endregion ...

     //The operand is a 32-bit metadata string token.
     case OperandType.InlineString:
     {
      #region ...
      int token = ar.ReadInt32();
      string s = new String(schema[token].Name.ToCharArray());
      ar.OverwriteInt32(ilInfo.GetTokenFor(s), offset + opCode.Size); 
      #endregion ...
     }
     break;

     //The operand is a 32-bit metadata signature token.
     case OperandType.InlineSig:
     {
      #region ...
      throw new Exception("НАДО ПРОВЕРИТЬ InlineSig !!!");
      //int token = ar.ReadInt32();
      //OverwriteInt32(ilInfo.GetTokenFor(inlineSigInstruction.Signature),
      //inlineSigInstruction.Offset + inlineSigInstruction.OpCode.Size);

      #endregion ...
     }
     //break;

     //The operand is a 32-bit metadata token.
     case OperandType.InlineMethod:
     {
      #region ...
      int token = ar.ReadInt32();
      MethodBase mi;
      Type t = schema[token].DeclaringType; // ?? dstType;
      if(schema[token].Name.StartsWith(".ctor"))
       mi = t.GetConstructor(bf, null, schema[token].ParametersTypes, null);
      else
       mi = t.GetMethod(schema[token].Name, bf, null, schema[token].ParametersTypes, null);
      if(mi == null)
       throw new PulsarException("Не удалось определить MethodBase для операнда {0}:{1}", opCode, opCode.OperandType);
      ar.OverwriteInt32(ilInfo.GetTokenFor(mi.MethodHandle, t.TypeHandle), offset + opCode.Size); 
      #endregion ...
     }
     break;

     //The operand is a 32-bit metadata token.
     case OperandType.InlineField:
     {
      #region ...
      int token = ar.ReadInt32();
      FieldInfo fi = schema[token].DeclaringType.GetField(schema[token].Name, bf | BindingFlags.FlattenHierarchy);
      if(fi == null)
       throw new PulsarException("Не удалось определить FieldInfo для операнда {0}:{1}", opCode, opCode.OperandType);
      ar.OverwriteInt32(ilInfo.GetTokenFor(fi.FieldHandle), offset + opCode.Size); 
      #endregion ...
     }
     break;

     //The operand is a 32-bit metadata token.
     case OperandType.InlineType:
     {
      #region ...
      int token = ar.ReadInt32();
      Type t = schema[token].DeclaringType;
      ar.OverwriteInt32(ilInfo.GetTokenFor(t.TypeHandle), offset + opCode.Size); 
      #endregion ...
     }
     break;

     //The operand is a FieldRef, MethodRef, or TypeRef token.
     case OperandType.InlineTok:
     {
      #region ...
      throw new Exception("НАДО ПРОВЕРИТЬ InlineTok !!!");
      //int token = ar.ReadInt32();
      //     MemberInfo mi = inlineTokInstruction.Member;
      //int token = 0;
      //if(mi.MemberType == MemberTypes.TypeInfo || mi.MemberType == MemberTypes.NestedType)
      //{
      // Type type = mi as Type;
      // token = ilInfo.GetTokenFor(type.TypeHandle);
      //}
      //else if(mi.MemberType == MemberTypes.Method || mi.MemberType == MemberTypes.Constructor)
      //{
      // MethodBase m = mi as MethodBase;
      // token = ilInfo.GetTokenFor(m.MethodHandle, m.DeclaringType.TypeHandle);
      //}
      //else if(mi.MemberType == MemberTypes.Field)
      //{
      // FieldInfo f = mi as FieldInfo;
      // //CLR BUG: token = ilInfo.GetTokenFor(f.FieldHandle, f.DeclaringType.TypeHandle);
      // token = ilInfo.GetTokenFor(f.FieldHandle);
      //}

      //OverwriteInt32(token,
      //   inlineTokInstruction.Offset + inlineTokInstruction.OpCode.Size);

      #endregion ...
     }
     //break;

     //The operand is the 32-bit integer argument to a switch instruction.
     case OperandType.InlineSwitch:
     {
      #region ...
      throw new Exception("НАДО ПРОВЕРИТЬ InlineSwitch !!!");
      //Int32 cases = ar.ReadInt32();
      //Int32[] deltas = new Int32[cases];
      //for(Int32 i = 0; i < cases; i++)
      // deltas[i] = ar.ReadInt32();

      #endregion ...
     }
     //break;
     default: throw new BadImageFormatException("Unexpected OperandType " + opCode.OperandType);
    }

   }
  }
  //*************************************************************************************
  #region << private class ArrayReader >>
  private class ArrayReader
  {
   private byte[] arr = null;
   private int offset = 0;

   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   #region << Properties >>
   public bool CanRead { get; private set; }
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   public int Position
   {
    get { return offset; }
   }
   #endregion << Properties >>
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


   //-------------------------------------------------------------------------------------
   #region << Constructors >>
   /// <summary>
   /// Конструктор по умолчанию.
   /// </summary>
   private ArrayReader() { }
   //-------------------------------------------------------------------------------------
   /// <summary>
   /// Инициализирующий конструктор.
   /// </summary>
   public ArrayReader(byte[] arr)
    : this()
   {
    this.arr = arr;
    CanRead =  offset < arr.Length;
   }
   #endregion << Constructors >>
   //-------------------------------------------------------------------------------------
   #region << Read Methods >>
   public Byte ReadByte()
   {
    CanRead = offset + 1 < arr.Length;

    return arr[offset++];
   }
   //-------------------------------------------------------------------------------------
   public SByte ReadSByte()
   {
    return (SByte)ReadByte();
   }
   //-------------------------------------------------------------------------------------
   public UInt16 ReadUInt16()
   {
    int pos = offset;
    offset += 2;
    return BitConverter.ToUInt16(arr, pos);
   }
   //-------------------------------------------------------------------------------------
   public UInt32 ReadUInt32()
   {
    int pos = offset;
    offset += 4;
    return BitConverter.ToUInt32(arr, pos);
   }
   //-------------------------------------------------------------------------------------
   public UInt64 ReadUInt64()
   {
    int pos = offset;
    offset += 8;
    return BitConverter.ToUInt64(arr, pos);
   }
   //-------------------------------------------------------------------------------------
   public Int32 ReadInt32()
   {
    int pos = offset;
    offset += 4;
    return BitConverter.ToInt32(arr, pos);
   }
   //-------------------------------------------------------------------------------------
   public Int64 ReadInt64()
   {
    int pos = offset;
    offset += 8;
    return BitConverter.ToInt64(arr, pos);
   }
   //-------------------------------------------------------------------------------------
   public Single ReadSingle()
   {
    int pos = offset;
    offset += 4;
    return BitConverter.ToSingle(arr, pos);
   }
   //-------------------------------------------------------------------------------------
   public Double ReadDouble()
   {
    int pos = offset;
    offset += 8;
    return BitConverter.ToDouble(arr, pos);
   }
   #endregion
   //-------------------------------------------------------------------------------------
   public void OverwriteInt32(int value, int pos)
   {
    arr[pos++] = (byte)value;
    arr[pos++] = (byte)(value >> 8);
    arr[pos++] = (byte)(value >> 16);
    arr[pos++] = (byte)(value >> 24);
   }
  } 
  #endregion << private class ArrayReader >>
  //*************************************************************************************
  #region << public class DiscoverMemberInfo >>
  /// <summary>
  /// Класс информации о члене класса.
  /// </summary>
  [Serializable]
  public class DiscoverMemberInfo
  {
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   #region << Properties >>
   /// <summary>
   /// Имя члена
   /// </summary>
   public string Name { get; set; }
   /// <summary>
   /// Тип, определяющий член класса
   /// </summary>
   public Type DeclaringType { get; set; }
   /// <summary>
   /// Generic аргументы
   /// </summary>
   public Type[] GenericArguments { get; set; }
   /// <summary>
   /// Типы параметров
   /// </summary>
   public Type[] ParametersTypes { get; set; }
   #endregion << Properties >>
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   #region << Constructors >>
   /// <summary>
   /// Конструктор по умолчанию.
   /// </summary>
   public DiscoverMemberInfo()
   {
    GenericArguments = Type.EmptyTypes;
    //DeclaringTypeGenericArguments = Type.EmptyTypes;
    ParametersTypes = Type.EmptyTypes;
   }
   //-------------------------------------------------------------------------------------
   /// <summary>
   /// Инициализирующий конструктор.
   /// </summary>
   public DiscoverMemberInfo(string name)
    : this()
   {
    Name = name;
   }
   #endregion << Constructors >>
   //-------------------------------------------------------------------------------------
   #region << Methods >>
   internal Type[] GetParametesTypes(ParameterInfo[] pi)
   {
    Type[] ts = new Type[pi.Length];
    for(int a = 0; a < pi.Length; a++)
     ts[a] = pi[a].ParameterType;
    return ts;
   }
   #endregion << Methods >>
   //-------------------------------------------------------------------------------------
  }
  #endregion public class DiscoverMemberInfo
  //*************************************************************************************
  #region << public class CodeQuery >>
  /// <summary>
  /// Класс кодового запроса к объекту Пульсара.
  /// </summary>
  [Serializable]
  public class CodeQuery
  {
   [NonSerialized]
   private string codeText = null;
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   #region << Properties >>
   /// <summary>
   /// Тип объекта, для которого предназначен кодовый запрос.
   /// </summary>
   public string ObjectType { get; set; }
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   /// <summary>
   /// Схема используемых членов типа.
   /// </summary>
   public Dictionary<int, DiscoverMemberInfo> Schema { get; set; }
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   /// <summary>
   /// Тело кодового запроса.
   /// </summary>
   public byte[] Body { get; set; }
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   /// <summary>
   /// Типы используемых локальных переменных.
   /// </summary>
   public List<KeyValuePair<Type, bool>> LocalVariables { get; set; }
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   /// <summary>
   /// Текст кодового запроса.
   /// </summary>
   public string CodeText { get { return codeText;} set { codeText = value; } }
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   internal int MaxStackSize { get; set; }
   #endregion << Properties >>
   //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
   //-------------------------------------------------------------------------------------
   #region << Constructors >>
   /// <summary>
   /// Конструктор по умолчанию.
   /// </summary>
   internal CodeQuery() { }
   #endregion << Constructors >>
   //-------------------------------------------------------------------------------------
  }
  #endregion << public class CodeQuery >>
 }
}
