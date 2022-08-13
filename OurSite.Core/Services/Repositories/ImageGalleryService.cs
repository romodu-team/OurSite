using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OurSite.Core.DTOs.ImageGalleryDtos;
using OurSite.Core.Services.Interfaces;
using OurSite.Core.Utilities;
using OurSite.DataLayer.Entities.ImageGalleries;
using OurSite.DataLayer.Entities.WorkSamples;
using OurSite.DataLayer.Interfaces;

namespace OurSite.Core.Services.Repositories;

public class ImageGalleryService : IimageGalleryService
{
    private IGenericReopsitories<ImageGallery> ImageGalleryRepository;
    private IGenericReopsitories<WorkSample> WorkSampleRepository;
    public ImageGalleryService(IGenericReopsitories<WorkSample> WorkSampleRepository,IGenericReopsitories<ImageGallery> ImageGalleryRepository)
    {
        this.ImageGalleryRepository=ImageGalleryRepository;
        this.WorkSampleRepository=WorkSampleRepository;
    }

    public void Dispose()
    {
        ImageGalleryRepository.Dispose();
        WorkSampleRepository.Dispose();
    }

    public async Task<ResAddImageToGallery> AddImageToGallery(SiteSections SiteSection, long WorkSampleId, IFormFile Image,string? alt)
    {   //check if siteSection is valid
       if(!SiteSections.IsDefined(SiteSection))
            return ResAddImageToGallery.SiteSectionNotValid;
        //check if workspace is exist
        var workspace= await WorkSampleRepository.GetEntity(WorkSampleId);
        if(workspace is null)
            return ResAddImageToGallery.worksampleNotFound;
        //if file is valid
        if(Image is not null)
        {
            //upload image to server
            var result =await FileUploader.UploadFile(PathTools.ImageGallery,Image,10);
            switch (result.Status)
            {
                case resFileUploader.Failure:
                    return ResAddImageToGallery.Failure;
                case resFileUploader.ToBig:
                    return ResAddImageToGallery.ToBig;
                case resFileUploader.InvalidExtention:
                    return ResAddImageToGallery.InvalidExtention;
                case resFileUploader.Success:
                    //add imageGallery record to database

                    var imageGallery= new ImageGallery()
                    {
                        ImageName=result.FileName,
                    IsRemove=false,
                    SiteSection=SiteSection,
                    SectionId=WorkSampleId,
                    ImagePath=PathTools.GetImageGallery+result.FileName,
                    ImageAlt=alt
                    };
                    try
                    {
                         await ImageGalleryRepository.AddEntity(imageGallery);
                         await ImageGalleryRepository.SaveEntity();
                         return ResAddImageToGallery.Success;
                    }
                    catch (System.Exception)
                    {
                        
                        return ResAddImageToGallery.Failure;
                    }
                   
                default:
                    return ResAddImageToGallery.Failure;
            }
        }
        return ResAddImageToGallery.NoContent;
      
    }

    public async Task<ResDeleteImage> DeleteImageFromGallery(long ImageId)
    {
        //check if image is exist
        var image = await ImageGalleryRepository.GetEntity(ImageId);
        if(image is null)
            return ResDeleteImage.NotFound;
        //check if image successfully deleted
        bool res = await ImageGalleryRepository.DeleteEntity(image.Id);
        if(res){
            await ImageGalleryRepository.SaveEntity();
            return ResDeleteImage.Success;
        }
        return ResDeleteImage.Faild;
    }



    public async Task<List<GetGalleryDto>> GetActiveGalleryByWorkSampleId(long WorkSampleId)
    {
       var gallery =await ImageGalleryRepository.GetAllEntity().Where(g=>g.SiteSection==SiteSections.WorkSamples && g.SectionId==WorkSampleId && g.IsRemove==false).ToListAsync();
       List<GetGalleryDto> imagePaths= new List<GetGalleryDto>();
       foreach (var image in gallery)
       {
            imagePaths.Add(new GetGalleryDto{Id=image.Id,ImageName=image.ImageName,ImagePath=image.ImagePath});
       }
       return imagePaths;
    }

    
}
