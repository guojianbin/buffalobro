using System;
using System.Data;
using System.Configuration;
using Buffalo.DB.EntityInfos;
using Buffalo.DB.BQLCommon;
using Buffalo.DB.BQLCommon.BQLConditionCommon;
using Buffalo.DB.PropertyAttributes;
namespace TestAddIn.BQLEntity
{
    [DataBaseAttribute("MyClass")]
    public partial class MyClass :BQLDataBaseHandle<MyClass> 
    {
    }
    
}
