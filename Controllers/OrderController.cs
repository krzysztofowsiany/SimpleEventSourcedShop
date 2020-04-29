using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleShop.Controllers.Orders;
using SimpleShop.Controllers.Requests;

namespace SimpleShop.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private AggregateService _aggregateService;

        public OrderController()
        {
            _aggregateService = new AggregateService();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOreder([FromBody] CreateOrderRequest request)
        {
            var order = new Order(request.OrderId);
            await _aggregateService.SaveAggregate(order, -1);
            System.IO.File.AppendAllLines("orders.txt", new[] {order.Id.ToString()});

            return Ok(order);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequest request)
        {
            var (version, order) = await _aggregateService.GetAggregate<Order>(request.OrderId.ToString());
            order.AddProduct(request.ProductId);
            await _aggregateService.SaveAggregate(order, version);

            return Ok(order);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveProduct([FromBody] RemoveProductRequest request)
        {
            var (version, order) = await _aggregateService.GetAggregate<Order>(request.OrderId.ToString());
            order.RemoveProduct(request.ProductId);
            await _aggregateService.SaveAggregate(order, version);

            return Ok(order);
        }

        [HttpPost("apply")]
        public async Task<IActionResult> AppplyClient([FromBody] ApplyClientRequest request)
        {
            var (version, order) = await _aggregateService.GetAggregate<Order>(request.OrderId.ToString());
            order.ApplyClient(request.ClientId);
            await _aggregateService.SaveAggregate(order, version);

            return Ok(order);
        }


        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmOrder([FromBody] ConfirmOrderRequest request)
        {
            var (version, order) = await _aggregateService.GetAggregate<Order>(request.OrderId.ToString());
            order.Confirm();
            await _aggregateService.SaveAggregate(order, version);

            return Ok(order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetState(string id)
        {
            var (version, order) = await _aggregateService.GetAggregate<Order>(id);
            await _aggregateService.SaveAggregate(order, version);
            return Ok(order);
        }
    }
}