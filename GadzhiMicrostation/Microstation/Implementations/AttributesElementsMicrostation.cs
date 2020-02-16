using GadzhiMicrostation.Models.Enum;
using MicroStationDGN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GadzhiMicrostation.Microstation.Implementations
{
    /// <summary>
    /// Обработка аттрибутов элементов Microstation
    /// </summary>
    public static class AttributesElementsMicrostation
    {
        /// <summary>
        /// Получить значение аттрибута через его ID номер
        /// </summary>       
        public static string GetAttributeById(Element element, ElementAttributes attributeId)
        {
            string attributeName = String.Empty;

            var dataBlocks = element?.GetUserAttributeData((int)ElementAttributes.AttributesArray).
                                      Cast<DataBlock>();
            var dataBlockFind = dataBlocks?.FirstOrDefault(data =>
            {
                short attributeIdFromDataBlock = 0;
                data.CopyInteger(attributeIdFromDataBlock, false);

                return attributeIdFromDataBlock == (int)attributeId;
            });

            dataBlockFind?.CopyString(attributeName, false);

            return attributeName;
        }

        //    Public Sub GetAttrByAttrNum(ByRef ms As MicroStationDGN.Application, ByRef oshi As Boolean, ByVal Elem As Element, ByVal AttrNum As Integer, ByRef AttrS As String, ByRef errorList() As String)
        //    Try
        //        Dim index As Integer
        //        Dim ele As Element
        //        Dim dblk()
        //        Dim Value As String
        //        Dim AttrNum1 As Integer

        //        'ReDim Attrs(0)
        //        ele = Elem
        //        dblk = ele.GetUserAttributeData(attrId)

        //        'ReDim Attrs(LBound(dblk) To UBound(dblk))
        //        For index = LBound(dblk) To UBound(dblk)
        //            TransferBlock(ms, oshi, dblk(index), Value, AttrNum1, False, errorList)
        //            If AttrNum = AttrNum1 Then
        //                AttrS = Value
        //            End If
        //        Next index
        //    Catch ex As Exception
        //        DwgSpecial.ErrorList(ms.ActiveDesignFile.FullName, "GetAttrByAttrNum", DwgSpecial.PapkaToConvert, oshi, TypeOfOshi, Patherror, ImaPolZ, errorList, True)
        //    End Try
        //End Sub
    }
}
