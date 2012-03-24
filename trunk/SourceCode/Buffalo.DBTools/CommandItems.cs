using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using EnvDTE80;
using System.IO;

namespace Buffalo.DBTools
{
    /// <summary>
    /// �˵���
    /// </summary>
    public class CommandItems
    {
        private DTE2 _applicationObject;
        private AddIn _addInInstance;
        Commands2 _commands = null;
        object commandEntityTools =null;
        object commandDB = null;
        Command commandEntity = null;
        Command commandCreater = null;
        Command commandROM = null;
        Command commandSummary = null;
        Command commandDBAll = null;
        Command commandDBSet = null;
        Command commandEntityUpdate = null;
        object[] _contextGUIDS = null;

        public CommandItems() 
        {

            

        }

        /// <summary>
        /// ��װ�˵�
        /// </summary>
        public void Install(DTE2 applicationObject, AddIn addInInstance, Commands2 commands) 
        {
            _contextGUIDS = new object[] { };
            _applicationObject = applicationObject;
            _addInInstance = addInInstance;
            _commands = commands;

            Microsoft.VisualStudio.CommandBars.CommandBar calssComm = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)[CommandBarId.ClassDesignerContextMenu];
            Microsoft.VisualStudio.CommandBars.CommandBar designerComm = ((Microsoft.VisualStudio.CommandBars.CommandBars)_applicationObject.CommandBars)[CommandBarId.ClassDiagramContextMenu];
            commandEntityTools = AddPopUp("Buffalo����", calssComm, 1);
            commandDB = AddPopUp("Buffalo����", designerComm, 1);
            commandEntity = AddToCommand("BuffaloEntityConfig", "����ʵ��", "����Buffaloʵ��", true, 523);
            commandCreater = AddToCommand("BuffaloDBCreater", "ʵ�嵽��", "ͨ��Buffaloʵ�����ɵ����ݿ�ı�", true, 295);
            commandROM = AddToCommand("BuffaloDBToEntity", "��ʵ��", "ͨ�����ݲ�ı�����Buffaloʵ��", true, 292);
            commandSummary = AddToCommand("BuffaloShowHideSummery", "��ʾ/����ע��", "��ʾ/����ע��", true, 192);
            commandDBAll = AddToCommand("BuffaloDBCreateAll", "�������ݿ�", "�������ݿ�", true, 577);
            commandDBSet = AddToCommand("BuffaloDBSet", "���ò���", "�������ݿ�����ɲ���", true, 611);
            commandEntityUpdate = AddToCommand("BuffaloUpdateEntityByDB", "����ʵ��", "�����ݿ�����ֶθ��µ�ʵ��", true, 59);
            //����Ӧ�ڸ�����Ŀؼ���ӵ���˵�
            if ((commandEntityTools != null) && (calssComm != null))
            {

                //command.AddControl(calssComm, 1);
                commandEntity.AddControl(commandEntityTools, 1);
                commandCreater.AddControl(commandEntityTools, 2);
                commandEntityUpdate.AddControl(commandEntityTools, 3);
            }
            //����Ӧ�ڸ�����Ŀؼ���ӵ�����ͼ�˵�
            if ((commandDB != null) && (designerComm != null))
            {
                commandROM.AddControl(commandDB, 1);
                commandSummary.AddControl(commandDB, 2);
                commandDBAll.AddControl(commandDB, 3);
                commandDBSet.AddControl(commandDB, 4);
                
            }

        }

        /// <summary>
        /// ж�ز˵�
        /// </summary>
        public void UnInstall() 
        {
            if (commandEntity != null)
            {
                commandEntity.Delete();
                commandEntity = null;
            }
            if (commandCreater != null)
            {
                commandCreater.Delete();
                commandCreater = null;
            }
            if (commandROM != null)
            {
                commandROM.Delete();
                commandROM = null;
            }
            if (commandSummary != null)
            {
                commandSummary.Delete();
                commandSummary = null;
            }
            if (commandDBAll != null)
            {
                commandDBAll.Delete();
                commandDBAll = null;
            }
            if (commandDBSet != null)
            {
                commandDBSet.Delete();
                commandDBSet = null;
            }
            if (commandEntityUpdate != null)
            {
                commandEntityUpdate.Delete();
                commandEntityUpdate = null;
            }
            if (_applicationObject != null && commandDB != null)
            {
                _applicationObject.Commands.RemoveCommandBar(commandDB);
                commandDB = null;
            }
            if (_applicationObject != null && commandEntityTools != null)
            {
                _applicationObject.Commands.RemoveCommandBar(commandEntityTools);
                commandEntityTools = null;
            }

            
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="name"></param>
        /// <param name="buttonText"></param>
        /// <param name="tooltip"></param>
        /// <param name="msoButton"></param>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private Command AddToCommand(string name, string buttonText, string tooltip, bool msoButton, object bitmap)
        {
            return _commands.AddNamedCommand2(_addInInstance, name, buttonText, tooltip, msoButton, bitmap,
                ref _contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported + (int)vsCommandStatus.vsCommandStatusEnabled,
                (int)vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton);
        }

        /// <summary>
        /// ��ӵ����˵�
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        private object AddPopUp(string buttonText, Microsoft.VisualStudio.CommandBars.CommandBar parent, int postion)
        {
            object command = _commands.AddCommandBar(buttonText, 
                vsCommandBarType.vsCommandBarTypeMenu, parent, postion);
            return command;
        }
       
    }
}
