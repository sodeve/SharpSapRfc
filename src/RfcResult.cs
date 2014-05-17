﻿using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharpSapRfc
{
    public class RfcResult
    {
        private RfcStructureMapper mapper;
        private IRfcFunction function;

        internal RfcResult(IRfcFunction function, RfcStructureMapper mapper)
        {
            this.mapper = mapper;
            this.function = function;
        }

        public T GetOutput<T>(string name)
        {
            object returnValue = function.GetValue(name);
            if (returnValue is IRfcStructure)
                return mapper.FromStructure<T>(returnValue as IRfcStructure);

            return (T)Convert.ChangeType(function.GetValue(name), typeof(T));
        }

        public IEnumerable<T> GetTable<T>(string name)
        {
            IRfcTable table = this.function.GetTable(name);
            List<T> returnTable = new List<T>(table.RowCount);
            for (int i = 0; i < table.RowCount; i++)
                returnTable.Add(mapper.FromStructure<T>(table[i]));

            return returnTable;
        }
    }
}