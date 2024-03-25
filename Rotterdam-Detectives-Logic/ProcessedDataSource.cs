using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RotterdamDetectives_DataInterface;
using RotterdamDetectives_LogicInterface;

namespace RotterdamDetectives_Logic
{
    public class ProcessedDataSource: IProcessedDataSource
    {
        private readonly IDataSource _dataSource;

        public ProcessedDataSource(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }
    }
}
