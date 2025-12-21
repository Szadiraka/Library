using Blob.Application.Dtos;
using Blob.Domain.Entities;

namespace Blob.Api.Mappers
{
    public static  class BucketMapper
    {
        public static BucketDto ToDto(Bucket bucket)
        {               
            return new BucketDto()
            {
                Id = bucket.Id,
                Name = bucket.Name,              
               CreatedAt = bucket.CreatedAt,
               UpdatedAt = bucket.UpdatedAt
            };
        }


        public static List<BucketDto> ToDtoList(List<Bucket> bukets)
        {
            return bukets.Select(x => ToDto(x)).ToList();
        }


        public static Bucket ToEntity(BucketDto dto)
        {
            return new Bucket()
            {                
                Name = dto.Name,
                CreatedAt = dto.CreatedAt                
            };
        }
    }
}
