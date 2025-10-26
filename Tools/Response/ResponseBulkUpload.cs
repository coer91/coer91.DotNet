namespace coer91.Tools
{
    public class ResponseBulkUpload<T> : ResponseDTOBuilder<BulkUploadDTO<T>>
    {
        public override BulkUploadDTO<T> Data { get; set; }
    }
}