namespace BTL_QuanLyBanDienThoai.Services.Interfaces
{
    public interface IBufferedFileUploadService
    {
        Task<bool> UploadFile(IFormFile file, string link);
    }
}
