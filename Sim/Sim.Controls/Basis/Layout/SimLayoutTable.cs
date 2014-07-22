using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sim.Controls
{
	public class SimLayoutTable : TableLayoutPanel
	{

		//-------------------------------------------------------------------------------------
		#region << Constructors >>
		/// <summary>
		/// Конструктор по умолчанию.
		/// </summary>
		public SimLayoutTable() : base()
		{

		}
		#endregion << Constructors >>
		//-------------------------------------------------------------------------------------
		#region << Methods >>
		public override Size GetPreferredSize(Size proposedSize)
		{

		 Size s;
			if(Dock != DockStyle.None && AutoSize )
			 s = base.GetPreferredSize(new Size(1,1));
			else
				s = base.GetPreferredSize(proposedSize);
			return s;
		}
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			if(Dock != DockStyle.None && AutoSize )
			{
		  Size	s = base.GetPreferredSize(new Size(1, 1));
			 base.SetBoundsCore(x, y, s.Width, s.Height, specified);
			}
			else
				base.SetBoundsCore(x, y, width, height, specified);
		}
		#endregion << Methods >>
	}
}
