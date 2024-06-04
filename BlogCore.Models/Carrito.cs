using System.Collections.Generic;
using BlogCore.Models.ViewModels;

namespace BlogCore.Models
{
    public class Carrito
    {
        public List<ItemCarrito> Items { get; set; } = new List<ItemCarrito>();

        public void AgregarItem(ItemCarrito item)
        {
            var itemExistente = Items.FirstOrDefault(i => i.ArticuloId == item.ArticuloId);
            if (itemExistente != null)
            {
                itemExistente.Cantidad += item.Cantidad;
            }
            else
            {
                Items.Add(item);
            }
        }

        public void RemoverItem(int articuloId)
        {
            var item = Items.FirstOrDefault(i => i.ArticuloId == articuloId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }
    }



}
