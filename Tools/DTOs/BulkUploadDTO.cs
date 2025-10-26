﻿namespace coer91.Tools
{
    public class BulkUploadDTO<T>
    {
        public List<T> RowsUploaded { get; set; } = [];
        public List<T> RowsExisting { get; set; } = [];
        public List<T> RowsIssues { get; set; } = [];
    }
}