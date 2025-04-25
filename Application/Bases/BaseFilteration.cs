namespace Application.Bases
{
    public class BaseFilteration
    {
        public int Length { get; set; } 
        public int Page { get; set; }
        public string[]? OrderBy { get; set; } = [];
        public string[]? Select { get; set; } = [];
        public long FromDate { get; set; }
        public long ToDate { get; set; }
        public string? SearchText { get; set; }
    }
}
