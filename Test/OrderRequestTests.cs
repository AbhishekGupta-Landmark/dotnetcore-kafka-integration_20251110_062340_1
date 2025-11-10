using Xunit;
using Api.Models;

namespace Api.Tests.Models
{
    public class OrderRequestTests
    {
        [Fact]
        public void OrderRequest_ValidData_ShouldCreateInstance()
        {
            var order = new OrderRequest
            {
                id = 1,
                productname = "Test Product",
                quantity = 5,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.Equal(1, order.id);
            Assert.Equal("Test Product", order.productname);
            Assert.Equal(5, order.quantity);
            Assert.Equal(OrderStatus.IN_PROGRESS, order.status);
        }

        [Fact]
        public void OrderRequest_AllOrderStatuses_ShouldBeValid()
        {
            var statuses = new[] 
            {
                OrderStatus.IN_PROGRESS,
                OrderStatus.COMPLETED,
                OrderStatus.REJECTED
            };

            foreach (var status in statuses)
            {
                var order = new OrderRequest { status = status };
                Assert.Equal(status, order.status);
            }
        }

        [Fact]
        public void OrderRequest_NegativeQuantity_ShouldBeAllowed()
        {
            var order = new OrderRequest
            {
                id = 1,
                productname = "Test Product",
                quantity = -5,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.Equal(-5, order.quantity);
        }

        [Fact]
        public void OrderRequest_EmptyProductName_ShouldBeAllowed()
        {
            var order = new OrderRequest
            {
                id = 1,
                productname = string.Empty,
                quantity = 1,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.Equal(string.Empty, order.productname);
        }

        [Fact]
        public void OrderRequest_NullProductName_ShouldBeAllowed()
        {
            var order = new OrderRequest
            {
                id = 1,
                productname = null,
                quantity = 1,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.Null(order.productname);
        }

        [Fact]
        public void OrderRequest_ZeroId_ShouldBeAllowed()
        {
            var order = new OrderRequest
            {
                id = 0,
                productname = "Test Product",
                quantity = 1,
                status = OrderStatus.IN_PROGRESS
            };

            Assert.Equal(0, order.id);
        }

        [Fact]
        public void OrderRequest_DefaultStatus_ShouldBeValid()
        {
            var order = new OrderRequest();
            Assert.Equal(0, order.id);
            Assert.Null(order.productname);
            Assert.Equal(0, order.quantity);
            Assert.Equal(OrderStatus.IN_PROGRESS, order.status);
        }

        [Fact]
        public void OrderRequest_MultipleInstances_ShouldHaveIndependentValues()
        {
            var order1 = new OrderRequest { id = 1, productname = "Product1", quantity = 5, status = OrderStatus.IN_PROGRESS };
            var order2 = new OrderRequest { id = 2, productname = "Product2", quantity = 10, status = OrderStatus.COMPLETED };

            Assert.NotEqual(order1.id, order2.id);
            Assert.NotEqual(order1.productname, order2.productname);
            Assert.NotEqual(order1.quantity, order2.quantity);
            Assert.NotEqual(order1.status, order2.status);
        }
    }
}