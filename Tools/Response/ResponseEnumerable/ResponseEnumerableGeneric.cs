namespace coer91.Tools
{
    public class ResponseList<T> : ResponseEnumerableBuilder<T>
    {
        public override IEnumerable<T> Data { get; set; }
    }
} 