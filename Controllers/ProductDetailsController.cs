using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using BTL_QuanLyBanDienThoai.Data;
using BTL_QuanLyBanDienThoai.Models;
using Microsoft.EntityFrameworkCore;
using BTL_QuanLyBanDienThoai.Models.ViewModel;

namespace BTL_QuanLyBanDienThoai.Controllers
{

    public class ProductDetailsController : Controller
    {
        private readonly QLBanDienThoaiContext _context;

        public ProductDetailsController(QLBanDienThoaiContext context)
        {
            _context = context;
        }

        [Route("home/products/detail/{id}")]

        public IActionResult Index(int ? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound(); // Handle the case where the product is not found
            }

            var productAttributeValues = _context.ProductAttributeValues
                .Join(_context.AttributeValues, pav => pav.AttributeValueId, av => av.Id, (pav, av) => new { pav, av })
                .Join(_context.Attrs, joined => joined.av.AttributeId, a => a.Id, (joined, a) => new { joined, a })
                .Where(joined => joined.joined.pav.ProductId == id)
                .Select(joined => new
                {
                    ProductVariantId = joined.joined.pav.ProductVariantId,
                    AttributeId = joined.a.Id,
                    AttributeName = joined.a.Name,
                    AttributeCode = joined.a.Code,
                    AttributeValueId = joined.joined.av.Id,
                    AttributeValue = joined.joined.av.Name
                })
                .ToList();

            var productVariants = new Dictionary<int, Dictionary<int, Dictionary<string, string>>>();

            foreach (var productAttributeValue in productAttributeValues)
            {
                if (!productVariants.ContainsKey((int)productAttributeValue.ProductVariantId))
                {
                    productVariants[(int)productAttributeValue.ProductVariantId] = new Dictionary<int, Dictionary<string, string>>();
                }

                var variant = productVariants[(int)productAttributeValue.ProductVariantId];
                variant[(int)productAttributeValue.AttributeId] = new Dictionary<string, string>
                {
                    { "attr_name", productAttributeValue.AttributeName },
                    { "attr_value", productAttributeValue.AttributeValue },
                    { "attr_code", productAttributeValue.AttributeCode }
                };
            }

            var categoryProducts = _context.Products
                .Where(p => p.CategoryId == product.CategoryId)
                .OrderBy(x => Guid.NewGuid())
                .Take(5)
                .ToList();

            var attrNames = productAttributeValues
                .Select(pa => pa.AttributeName)
                .ToArray(); 

            var productImages = _context.ProductImages.Where(i => i.ProductId == id).ToList();

            return View(new ProductDetailViewModel
            {
                Product = product,
                ProductVariants = productVariants,
                CategoryProducts = categoryProducts,
                AttrNames = attrNames,
                productImages = productImages
            });
        }
    }
}

