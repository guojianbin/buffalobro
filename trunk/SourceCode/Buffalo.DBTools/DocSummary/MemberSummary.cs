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
@version ����ʱ�䣺2011-12-1
��ʾ��ͼע��
*/
namespace Buffalo.DBTools.DocSummary
{
    internal class MemberSummary : ShapeField
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
                        if (itemDrawInfo.Disabled)
                        {
                            continue;
                        }
                        string[] strArray = itemDrawInfo.Text.Split(new char[] { ':', '(', '{', '<' });
                        Member menberByName = this.GetMenberByName(parentShape.ParentShape, strArray[0].Trim());
                        if ((menberByName == null) || string.IsNullOrEmpty(menberByName.DocSummary))
                        {
                            continue;
                        }
                        string docSummary = menberByName.DocSummary;
                        if (((ClassDiagram)parentShape.Diagram).MembersFormat == MemberTextFormat.NameAndType)
                        {

                            string genericTypeName = "";
                            if (menberByName.MemberType is ClrStruct)
                            {
                                switch (menberByName.MemberTypeName)
                                {
                                    case "bool":
                                        break;
                                    case "bool?":
                                        genericTypeName = "�Ƿ�";
                                        break;

                                    case "int":
                                        break;
                                    case "int?":
                                        genericTypeName = "����";
                                        break;

                                    case "uint":
                                        break;
                                    case "uint?":
                                        genericTypeName = "������";
                                        break;

                                    case "byte":
                                        break;
                                    case "byte?":
                                        genericTypeName = "�ֽ�";
                                        break;

                                    case "byte[]":
                                        genericTypeName = "�ֽ�����";
                                        break;

                                    case "long":
                                        break;
                                    case "long?":
                                        genericTypeName = "������";
                                        break;

                                    case "System.DateTime":
                                        break;
                                    case "System.DateTime?":
                                        genericTypeName = "ʱ��";
                                        break;

                                    case "decimal":
                                        break;
                                    case "decimal?":
                                        genericTypeName = "��ȷС��";
                                        break;

                                    case "float":
                                        break;
                                    case "float?":
                                        genericTypeName = "������";
                                        break;
                                    default:
                                        genericTypeName = menberByName.MemberType.GenericTypeName;
                                        break;
                                }

                            }
                            else if ((menberByName.MemberType is ClrClass) || (menberByName.MemberType is ClrEnumeration))
                            {
                                if (menberByName.MemberType.Name == "String")
                                {
                                    genericTypeName = "����";
                                }
                                else if (menberByName.MemberType.Name == "List")
                                {
                                    genericTypeName = menberByName.MemberType.GenericTypeName;
                                }
                                else if (string.IsNullOrEmpty(menberByName.MemberType.DocSummary))
                                {
                                    genericTypeName = menberByName.MemberType.GenericTypeName;
                                }
                                else
                                {
                                    genericTypeName = menberByName.MemberType.DocSummary;
                                }
                            }
                            else
                            {
                                genericTypeName = menberByName.MemberTypeName;
                            }

                            docSummary =  docSummary + "��" + genericTypeName ;
                        }
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
                        string str=menberByName.Name+"("+docSummary+")";
                        float x = 0.4f;

                        e.Graphics.FillRectangle(this.BackBrush, x,(float)(0.245f+(height) * i), 
                            (float)(((float)parentShape.BoundingBox.Width) - 0.45f),
                            height);

                        e.Graphics.DrawString(str, font, this.SumerBrush, x, (float)(0.250f + (height) * i));
                        

                    }
                }
            }
        }

        private Font GetFont(DiagramClientView View)
        {
            if (View == null)
            {
                return new Font("����", 9f, FontStyle.Regular);
            }
            return new Font("����", View.Font.Size * View.ZoomFactor, FontStyle.Regular);
        }

        private Member GetMenberByName(ShapeElement clsShape, string Mname)
        {
            ClrType associatedType = null;
            if (clsShape is ClrClassShape)
            {
                associatedType = (clsShape as ClrClassShape).AssociatedType;
            }
            if (clsShape is ClrInterfaceShape)
            {
                associatedType = (clsShape as ClrInterfaceShape).AssociatedType;
            }
            if (associatedType != null)
            {
                foreach (Member member in (IEnumerable)associatedType.Members)
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
