﻿using RotterdamDetectives_LogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotterdamDetectives_Logic.Interface
{
    internal class Ticket : ITicket
    {
        public string Name { get; set; }

        public int Amount { get; set; }
    }
}
