namespace BackendUsers.Models
{
    
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public int CategoryId { get; set; }

            public DateTime StartTime { get; set; }

            public DateTime EndTime { get; set; }

            public string? Description { get; set; }

            public int Quantity { get; set; }

            public decimal Price { get; set; }

            public string? ImageURl { get; set; }



            public int? Energy { get; set; }

            public int? Carbohydrate { get; set; }

            public int? Sugar { get; set; }

            public int? Protein { get; set; }

            public int? Fiber { get; set; }

            public string? Fact { get; set; }

        public bool Deleted { get; set; } = false;
    }
    }


