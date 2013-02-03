using System;
using System.Collections.Generic;
using System.Text;
using Buffalo.Kernel.FastReflection;
using System.Reflection;

namespace Buffalo.DBTools.UIHelper.ModelLoader
{
    /// <summary>
    /// 代码生成的信息
    /// </summary>
    public class CodeGenInfo
    {
        public CodeGenInfo(Type classType)
        {
            _classType = classType;
            _codeClass = Activator.CreateInstance(_classType);
            MethodInfo info=classType.GetMethod("DoCompiler",FastValueGetSet.AllBindingFlags);
            _methodInfo = FastInvoke.GetMethodInvoker(info);
        }

        private Type _classType;

        public Type ClassType
        {
            get { return classType; }
        }

        private object _codeClass;

        private FastInvokeHandler _methodInfo;

        /// <summary>
        /// 运行编译函数并返回结果
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public string Invoke(EntityInfo entityInfo, UIConfigItem classConfig,
            List<UIModelItem> selectPropertys) 
        {
            return _methodInfo.Invoke(_codeClass, new object[] { entityInfo, classConfig, selectPropertys }) as string;
        }

    }
}
