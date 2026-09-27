using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopApp.Models
{
    internal class LogData
    {
        public LogData ()
        {
        }

        public LogData(int userId, int orderId, int machineId, string sourcingTime, string preparationTime, string servedTime)
        {
            UserId = userId;
            OrderId = orderId;
            MachineId = machineId;
            SourcingTime = sourcingTime;
            PreparationTime = preparationTime;
            ServedTime = servedTime;
        }

        public int UserId { get; set; }

        public int OrderId { get; set; }

        public int MachineId { get; set; }

        public string SourcingTime { get; set; } = string.Empty;

        public string PreparationTime { get; set; } = string.Empty;

        public string ServedTime { get; set; } = string.Empty;
    }
}
