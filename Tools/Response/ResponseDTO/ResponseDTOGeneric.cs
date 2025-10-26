namespace coer91.Tools
{
    public class ResponseDTO<T> : ResponseDTOBuilder<T>
    {
        public override T Data { get; set; }
    }
} 