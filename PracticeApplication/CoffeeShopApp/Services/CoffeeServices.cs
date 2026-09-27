using System.Threading.Channels;
using CoffeeShopApp.Models;
using CoffeeShopApp.Repository;

namespace CoffeeShopApp.Services
{
    internal class CoffeeServices
    {
        private NotificationService notificationService;
        private OrderServices orderServices;
        private InventoryService inventoryService;
        private JsonLogger _logger;
        private LogDataLogger _logDataLogger;

        private readonly CancellationToken cts;
        private readonly Channel<Order> _orderChannel = Channel.CreateUnbounded<Order>(new UnboundedChannelOptions
        {
            SingleReader = false,
            SingleWriter = true,
        });

        public CoffeeServices(NotificationService notificationService, OrderServices orderServices, JsonLogger jsonLogger, InventoryService inventoryService, LogDataLogger logDataLogger, CancellationTokenSource cancellationTokenSource)
        {
            this.notificationService = notificationService;
            this.orderServices = orderServices;
            this._logger = jsonLogger;
            this.inventoryService = inventoryService;
            this._logDataLogger = logDataLogger;
            this.cts = cancellationTokenSource.Token;

            for (int i = 1; i <= 3; i++)
            {
                Machine machine = new Machine();
                machine.MachineId = i;
                machine.IsAvailable = true;
                _ = this.MachineWorker(machine);
            }
        }

        internal void PrepareCoffee(int userChoice, int currentUserId)
        {
            Order? order = orderServices.FetchOrderDetails(userChoice, currentUserId);
            if (order == null)
            {
                return;
            }

            if (this.inventoryService.ReduceStock(order.CoffeeOrdered))
            {
                this._orderChannel.Writer.TryWrite(order);
                notificationService.NotifyUser($"Order {order.OrderId} is queued for User {currentUserId}", currentUserId);

            }
            else
            {
                notificationService.NotifyUser($"Insufficient Stock for order {order.OrderId}", currentUserId);
            }
        }

        internal async Task MachineWorker(Machine machine)
        {
            await foreach (var order in this._orderChannel.Reader.ReadAllAsync(this.cts))
            {
                try
                {
                    machine.OrderId = order.OrderId;
                    machine.IsAvailable = false;
                    await this.StartPreparation(order, machine);
                }
                catch (Exception e)
                {
                    Console.Write($"{e.Message}");
                }
                finally
                {
                    machine.OrderId = null;
                    machine.IsAvailable = true;
                }
            }
        }

        internal async Task StartPreparation(Order order, Machine machine)
        {
            LogData log = new LogData();
            log.OrderId = order.OrderId;
            log.UserId = order.UserId;
            log.MachineId = machine.MachineId;
            await _logger.LogAsync(order.UserId, "MachineAssigned", $"Machine {machine.MachineId} assigned to Coffee {order.OrderId}", order.OrderId, machine.MachineId);

            log.SourcingTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            await _logger.LogAsync(order.UserId, "SourcingStarted", $"Coffee {order.OrderId} sourcing started", order.OrderId, machine.MachineId);
            notificationService.NotifyUser($"Coffee {order.OrderId} sourcing is started", order.UserId);
            await Task.Delay(order.CoffeeOrdered.SourcingTime, cts);

            log.PreparationTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            await _logger.LogAsync(order.UserId, "PreparationStarted", $"Coffee {order.OrderId} preparation started", order.OrderId, machine.MachineId);
            notificationService.NotifyUser($"Coffee {order.OrderId} preparation is started", order.UserId);
            await Task.Delay(order.CoffeeOrdered.PreparationTime, cts);

            log.ServedTime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            await _logger.LogAsync(order.UserId, "CoffeeReady", $"Coffee {order.OrderId} is ready for delivery", order.OrderId, machine.MachineId);
            notificationService.NotifyUser($"Coffee {order.OrderId} is ready for delivery", order.UserId);
            await _logDataLogger.LogAsync(log);
        }

        /*private Machine GetFreeMachine(int orderId) 
        { 
        Machine machine = machines.First(m => m.IsAvailable == true); 

        machine.IsAvailable = false; 
        machine.OrderId = orderId; 

        return machine; 
        } 

        private void ReleaseMachine(Machine machine) 
        { 
        machine.IsAvailable = true; 
        machine.OrderId = null; 
        }*/
    }
}