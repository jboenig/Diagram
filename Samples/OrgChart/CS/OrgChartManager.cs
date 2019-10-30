using System;
using System.Collections;
using System.ComponentModel;

using Syncfusion.Windows.Forms.Diagram;

namespace Syncfusion.OrgChart
{
	/// <summary>
	/// Summary description for OrgChartManager.
	/// </summary>
	public class OrgChartManager : LayoutManager
	{
		public OrgChartManager()
		{
		}

		public OrgChartManager(Model mdl) : base(mdl)
		{
		}

		public override bool UpdateLayout(object contextInfo)
		{
			bool changesMade = false;

			if (this.model != null)
			{
				ArrayList topRanking = new ArrayList();

				foreach (INode curNode in this.model.Nodes)
				{
					MemberSymbol memberSym = curNode as MemberSymbol;
					if (memberSym != null)
					{
						if (memberSym.Rank == 1)
						{
							topRanking.Add(memberSym);
						}
					}
				}

				int numTopRanking = topRanking.Count;

				if (numTopRanking > 0)
				{
					float range = this.model.Width / ((float) numTopRanking);
					float curLeft = this.model.Bounds.Left;
					float curRight = curLeft + range;

					foreach (MemberSymbol curMember in topRanking)
					{
						this.PositionMember(curMember, curLeft, curRight);
						curLeft = curRight;
						curRight = curRight + range;
					}
				}
			}

			return changesMade;
		}

		private void PositionMember(MemberSymbol memberSym, float left, float right)
		{
			int rank = memberSym.Rank;
			memberSym.Y = ((float) rank) * this.verticalSpacing;
			float symWidth = memberSym.Width;
			float rangeCenter = (left + right) / 2.0f;
			memberSym.X = rangeCenter - (symWidth / 2.0f);

			ICollection employees = memberSym.Employees;
			int numEmployees = employees.Count;

			if (numEmployees > 0)
			{
				float range = (right - left) / ((float) numEmployees);
				float curLeft = left;
				float curRight = curLeft + range;
				foreach (MemberSymbol employee in employees)
				{
					this.PositionMember(employee, curLeft, curRight);
					curLeft = curRight;
					curRight = curRight + range;
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
			Browsable(true),
			DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public float HorizontalSpacing
		{
			get
			{
				return this.horizontalSpacing;
			}
			set
			{
				this.horizontalSpacing = value;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		[
		Browsable(true),
		DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)
		]
		public float VerticalSpacing
		{
			get
			{
				return this.verticalSpacing;
			}
			set
			{
				this.verticalSpacing = value;
			}
		}

		private float horizontalSpacing = 200.0f;
		private float verticalSpacing = 140.0f;
	}
}
