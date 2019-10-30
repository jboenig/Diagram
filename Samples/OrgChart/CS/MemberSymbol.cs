using System;
using System.Collections;
using System.Diagnostics;

using Syncfusion.Windows.Forms.Diagram;

namespace Syncfusion.OrgChart
{
	/// <summary>
	/// Summary description for MemberSymbol.
	/// </summary>
	public class MemberSymbol : Symbol
	{
		private RoundRect rect = null;
		private Label nameLbl = null;
		private Label titleLbl = null;

		public MemberSymbol()
		{
			this.rect = new RoundRect(0, 0, 140, 60);
			this.rect.FillStyle.Color = System.Drawing.Color.Beige;
			this.AppendChild(this.rect);
			this.nameLbl = this.AddLabel("", BoxPosition.Center);
			this.nameLbl.OffsetY = -12;
			this.nameLbl.FontStyle.Style = System.Drawing.FontStyle.Bold;
			this.nameLbl.BackgroundStyle.Color = System.Drawing.Color.Transparent;
			this.titleLbl = this.AddLabel("", BoxPosition.Center);
			this.titleLbl.OffsetY = 12;
			this.titleLbl.BackgroundStyle.Color = System.Drawing.Color.Transparent;
		}

		public string MemberName
		{
			get
			{
				return this.nameLbl.Text;
			}
			set
			{
				this.nameLbl.Text = value;
				this.nameLbl.Size = this.nameLbl.SizeToText(this.Size);
			}
		}

		public string Title
		{
			get
			{
				return this.titleLbl.Text;
			}
			set
			{
				this.titleLbl.Text = value;
				this.titleLbl.Size = this.titleLbl.SizeToText(this.Size);
			}
		}

		public override bool AcceptConnection(Port sourcePort, Port targetPort)
		{
			bool accept = false;

			// First, determine which port belongs to the foreign port container.

			Port foreignPort = null;

			if (this.Ports.Contains(sourcePort) || this.CenterPort == sourcePort)
			{
				foreignPort = targetPort;
			}
			else if (this.Ports.Contains(targetPort) || this.CenterPort == targetPort)
			{
				foreignPort = sourcePort;
			}

			if (foreignPort == null)
			{
				return false;  // This should never happen
			}

			// Now get the foreign port container and check to see if it is
			// a Link object. If so, then check to see if the foreign port
			// is the link's tail port or head port. If it's the tail port,
			// then always allow the connection, because a member symbol can
			// have any number of edges leaving it. If it's the head port,
			// then only allow the connection if the member symbol has no
			// other edges entering it yet.

			IPortContainer foreignContainer = foreignPort.Container;
			if (foreignContainer != null)
			{
				Link link = foreignContainer as Link;
				if (link != null)
				{
					if (foreignPort == link.TailPort)
					{
						accept = true;
					}
					else
					{
						IGraphNode curNode = this as IGraphNode;

						if (curNode != null)
						{
							ICollection edgesEntering = curNode.EdgesEntering;
							if (edgesEntering.Count == 0)
							{
								accept = true;
							}
						}
					}
				}
			}

			return accept;
		}

		public int Rank
		{
			get
			{
				int rank = 1;

				IGraphNode thisNode = this as IGraphNode;
				IGraphNode curNode = thisNode;
				bool cyclicalPathFound = false;

				while (curNode != null && !cyclicalPathFound)
				{
					ICollection edgesEntering = curNode.EdgesEntering;
					if (edgesEntering.Count > 0)
					{
						IEnumerator enumEdgesEntering = edgesEntering.GetEnumerator();
						if (enumEdgesEntering != null)
						{
							enumEdgesEntering.MoveNext();
							IGraphEdge incomingEdge = enumEdgesEntering.Current as IGraphEdge;
							if (incomingEdge != null)
							{
								curNode = incomingEdge.FromNode;

								if (curNode == thisNode)
								{
									cyclicalPathFound = true;
								}
								else
								{
									rank++;
								}
							}
						}
					}
					else
					{
						curNode = null;
					}
				}

				return rank;
			}
		}

		public MemberSymbol Manager
		{
			get
			{
				MemberSymbol manager = null;
				IGraphNode thisNode = this as IGraphNode;
				IGraphNode fromNode;

				if (thisNode != null)
				{
					ICollection edgesEntering = thisNode.EdgesEntering;
					if (edgesEntering.Count > 0)
					{
						IEnumerator enumEdgesEntering = edgesEntering.GetEnumerator();
						if (enumEdgesEntering != null)
						{
							enumEdgesEntering.MoveNext();
							IGraphEdge incomingEdge = enumEdgesEntering.Current as IGraphEdge;
							if (incomingEdge != null)
							{
								fromNode = incomingEdge.FromNode;

								if (fromNode != null)
								{
									manager = fromNode as MemberSymbol;
								}
							}
						}
					}
				}

				return manager;
			}
		}

		public ICollection Employees
		{
			get
			{
				ArrayList employees = new ArrayList();

				IGraphNode thisNode = this as IGraphNode;

				if (thisNode != null)
				{
					ICollection edgesLeaving = thisNode.EdgesLeaving;
					if (edgesLeaving != null)
					{
						foreach (IGraphEdge curEdge in edgesLeaving)
						{
							MemberSymbol curEmployee = curEdge.ToNode as MemberSymbol;
							if (curEmployee != null)
							{
								employees.Add(curEmployee);
							}
						}
					}
				}

				return employees;
			}
		}
	}
}
