using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.EnterpriseTools.ClassDesigner.PresentationModel;
using System.Collections;
/** 
@author 289323612@qq.com
@version 创建时间：2011-12-1
显示类图注释
*/
namespace Buffalo.DBTools.DocSummary
{
    internal class EnumItemSummary : ShapeField
    {
        // Fields
        private Connect _FromAddin;
        private SolidBrush BackBrush = new SolidBrush(Color.White);
        private SolidBrush SumerBrush = new SolidBrush(Color.Black);

        // Methods
        public override void DoPaint(DiagramPaintEventArgs e, ShapeElement parentShape)
        {
            base.DoPaint(e, parentShape);
            Font font = this.GetFont(e.View);
            CDCompartment compartment = parentShape as CDCompartment;
            if (compartment != null)
            {
                ListField listField = null;
                foreach (ShapeField field2 in compartment.ShapeFields)
                {
                    if (field2 is ListField)
                    {
                        listField = field2 as ListField;
                        break;
                    }
                }
                if (listField != null)
                {
                    int itemCount = compartment.GetItemCount(listField);
                    for (int i = 0; i < itemCount; i++)
                    {
                        ItemDrawInfo itemDrawInfo = new ItemDrawInfo();
                        compartment.GetItemDrawInfo(listField, i, itemDrawInfo);
                        if (!itemDrawInfo.Disabled)
                        {
                            string[] strArray = itemDrawInfo.Text.Split(new char[] { ':', '(', '{' });
                            Member menberByName = this.GetMenberByName(parentShape.ParentShape, strArray[0].Trim());
                            if ((menberByName != null))
                            {
                                string docSummary = menberByName.DocSummary;
                                this.BackBrush.Color = Color.White;
                                this.SumerBrush.Color = Color.Black;
                                SelectedShapesCollection seleShapes = this._FromAddin.SelectedShapes;
                                if (seleShapes != null)
                                {
                                    foreach (DiagramItem item in seleShapes)
                                    {
                                        if (((item.Shape == compartment) && (item.Field == listField)) && (item.SubField.SubFieldHashCode == i))
                                        {
                                            this.BackBrush.Color = SystemColors.ActiveCaption;
                                            this.SumerBrush.Color = Color.White;
                                            break;
                                        }
                                    }
                                }
                                float height = 0.19f;
                                float recX = (float)itemDrawInfo.ImageMargin;//0.16435f
                                RectangleD bound = parentShape.BoundingBox;
                                string str = menberByName.Name;
                                if (!string.IsNullOrEmpty(docSummary)) 
                                {
                                     str+= "(" + docSummary + ")";
                                }
                                float x = 0f;

                                e.Graphics.FillRectangle(this.BackBrush, x, (float)(0.01f + (height) * i),
                                    (float)(((float)parentShape.BoundingBox.Width) - 0.45f),
                                    height);

                                e.Graphics.DrawString(str, font, this.SumerBrush, x, (float)(0.02f + (height) * i));
                                //e.Graphics.FillRectangle(this.BackBrush, (float)0.01f, (float)(0.018f + (0.16435f * i)), (float)(((float)parentShape.BoundingBox.Width) - 0.45f), (float)0.144f);
                                //e.Graphics.DrawString(docSummary, font, this.SumerBrush, (float)0.01f, (float)(0.02f + (0.16435f * i)));
                            }
                        }
                    }
                }
            }
        }

        private Font GetFont(DiagramClientView View)
        {
            if (View == null)
            {
                return new Font("宋体", 9f, FontStyle.Regular);
            }
            return new Font("宋体", View.Font.Size * View.ZoomFactor, FontStyle.Regular);
        }

        private Member GetMenberByName(ShapeElement clsShape, string Mname)
        {
            if (clsShape is ClrEnumerationShape)
            {
                foreach (Member member in (IEnumerable)(clsShape as ClrEnumerationShape).AssociatedType.Members)
                {
                    if (member.Name == Mname)
                    {
                        return member;
                    }
                }
            }
            return null;
        }

        // Properties
        public Connect FromAddin
        {
            get
            {
                return this._FromAddin;
            }
            set
            {
                this._FromAddin = value;
            }
        }
    }

 

}
