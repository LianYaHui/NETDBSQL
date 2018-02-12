using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FoxzyDBSql.Common
{
    public class InputParamterType
    {
        private object __InputParamter = null;
        private string __command = "";
        private CommandType __type;

        public InputParamterType(string command, object inputParamter, CommandType type)
        {
            __InputParamter = inputParamter;
            __command = command;
            __type = type;
        }
        public event OnInputIsDictionaryEvent OnInputIsDictionary;

        public event OnInputIsListEvent OnInputIsList;

        public event OnInputIsObjectEvent OnInputIsObject;

        public void InitCommand()
        {
            if (__InputParamter == null)
            {
                OnInputIsObject.Invoke(this, new DataParameterEventArgs(__command, null, __type));
                return;
            }


            if (__InputParamter is Dictionary<String, object> && OnInputIsDictionary != null)
            {
                OnInputIsDictionary.Invoke(this, new DataParameterEventArgs(__command, __InputParamter, __type));
                return;
            }

            if (__InputParamter is IEnumerable<IDataParameter> && OnInputIsList != null)
            {
                OnInputIsList.Invoke(this, new DataParameterEventArgs(__command, __InputParamter, __type));
                return;
            }

            if (OnInputIsObject != null)
                OnInputIsObject.Invoke(this, new DataParameterEventArgs(__command, __InputParamter, __type));
        }
    }
}
