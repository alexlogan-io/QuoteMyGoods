namespace QuoteMyGoods.Services
{
    public interface IBlobbingService
    {
        void UploadBlob<T>(string fileReference, T file);
        T GetBlob<T>(string reference);
    }
    public class BlobbingService : IBlobbingService
    {
        //private BlobStorage _blobStorage;

        public BlobbingService()
        {
           // _blobStorage = new BlobStorage(Startup.Configuration["BlobStorage:ConnectionString"]);
        }

        public void UploadBlob<T>(string fileReference, T file)
        {
            //_blobStorage.uploadBlob(fileReference, file);
        }

        public T GetBlob<T>(string reference)
        {
            //return _blobStorage.DownloadBlob<T>(reference);
            return default(T);
        }
    }
}
