using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Pulsar.Reflection;

namespace Sim.Shell
{
 /// <summary>
 /// 
 /// </summary>
	public partial class CtrlAssistInfo : UserControl
	{
		private dynamic _pers;

		public dynamic Person
		{
			get { return _pers; }
			set 
			{ 
			 _pers = value; 
				tlFName.Label2Text = (string)_pers.FName;
				tlIName.Label2Text = (string)_pers.IName;
				tlOName.Label2Text = (string)_pers.OName;
				tlType.Label2Text  = EnumTypeConverter.GetItemDisplayName((Enum)_pers.PersonType);
				tlRoles.Label2Text  = EnumTypeConverter.GetItemDisplayName((Enum)_pers.Roles);
			}
		}

		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		public CtrlAssistInfo()
		{
			InitializeComponent();
			BackColor = Color.Transparent;
		}
		#endregion << Constructors >>
	}
}
