using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using System.Reflection;
using Buffalo.GeneratorInfo;
using System.CodeDom.Compiler;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// 代码生成的信息
    /// </summary>
    public class CodeGenInfo
    {
        private string _code;
        /// <summary>
        /// 生成代码
        /// </summary>
        public string Code
        {
            get { return _code; }
        }


        public CodeGenInfo(Type classType, string code)
        {
            _classType = classType;
            _codeClass = Activator.CreateInstance(_classType);
            

            MethodInfo info = classType.GetMethod(CodesManger.DoCompilerName, FastValueGetSet.AllBindingFlags);
            _methodInfo = FastInvoke.GetMethodInvoker(info);
        }

        private Type _classType;

        public Type ClassType
        {
            get { return _classType; }
        }

        private object _codeClass;

        private FastInvokeHandler _methodInfo;

        /// <summary>
        /// 运行编译函数并返回结果
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Invoke(EntityInfo entityInfo, UIConfigItem classConfig,
            List<UIModelItem> selectPropertys, UIModelItem classInfo) 
        {
            GeneratorEntity entity = entityInfo.ToGeneratorEntity(classInfo);
            List<GenerateItem> lst = new List<GenerateItem>(selectPropertys.Count);
            foreach (UIModelItem item in selectPropertys) 
            {
                lst.Add(item.ToGeneratItem());
            }
            return _methodInfo.Invoke(_codeClass, new object[] { entity, lst }) as string;
        }
        

    }
}
