namespace THLapTrinhWeb.Models
{
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        public void AddItem(CartItem item)
        {
            var existingItem = Items.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoveItem(int productId)
        {
            Items.RemoveAll(i => i.ProductId == productId);
        }

        // Thêm method GetTotalQuantity
        public int GetTotalQuantity()
        {
            return Items.Sum(i => i.Quantity);
        }

        // Thêm method GetTotalPrice
        public decimal GetTotalPrice()
        {
            return Items.Sum(i => i.Price * i.Quantity);
        }

        // Thêm method Clear
        public void Clear()
        {
            Items.Clear();
        }

        // Thêm method UpdateQuantity
        public void UpdateQuantity(int productId, int quantity)
        {
            var item = Items.FirstOrDefault(i => i.ProductId == productId);
            if (item != null)
            {
                if (quantity <= 0)
                {
                    RemoveItem(productId);
                }
                else
                {
                    item.Quantity = quantity;
                }
            }
        }
    }
}