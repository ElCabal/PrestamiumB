﻿namespace Prestamium.Entities
{
    public class Client : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public int CellPhoneNumber { get; set; }
        public string Email { get; set; } = default!;
    }
}
