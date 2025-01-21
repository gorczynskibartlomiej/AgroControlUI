using AgroControlUI.DTOs.CropProtectionProducts;
using AgroControlUI.DTOs.FarmData;
using AgroControlUI.DTOs.Fertilizers;
using AgroControlUI.DTOs.FieldWorks;
using AgroControlUI.DTOs.ReferenceData;
using AgroControlUI.DTOs.UserManagement;
using AgroControlUI.Models.CropProtectionProducts;
using AgroControlUI.Models.FarmData;
using AgroControlUI.Models.Fertilizers;
using AgroControlUI.Models.FieldWorks;
using AgroControlUI.Models.ReferenceData;
using AgroControlUI.Models.UserManagement;
using AutoMapper;
namespace AgroControlApi.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Mapping to DTO

            CreateMap<ActiveIngredient, ActiveIngredientDto>();

            CreateMap<CropProtectionProductCategory, CropProtectionProductCategoryDto>();

            CreateMap<CropProtectionProductComponent, CropProtectionProductComponentDto>();

            CreateMap<CropProtectionProduct, CropProtectionProductDto>();


            CreateMap<AgriculturalEquipment, AgriculturalEquipmentDto>();

            CreateMap<AgriculturalEquipment, AgriculturalEquipmentDetailsDto>();

            CreateMap<CropRotationPlanner, CropRotationPlannerDto>();

            CreateMap<CropRotationPlanner, CropRotationPLannerDetailsDto>();

            CreateMap<Employee, EmployeeDto>();

            CreateMap<Farm, FarmDto>();

            CreateMap<FarmInventory, FarmInventoryDto>();

            CreateMap<Field, FieldDto>();

            CreateMap<GrainSilo, GrainSiloDto>();

            CreateMap<Purchase, PurchaseDto>();


            CreateMap<ChemicalElement, ChemicalElementDto>();

            CreateMap<FertilizerCategory, FertilizerCategoryDto>();

            CreateMap<FertilizerComponent, FertilizerComponentDto>();

            CreateMap<Fertilizer, FertilizerDto>();



            CreateMap<FieldWork, FieldWorkDto>();



            CreateMap<AgriculturalEquipmentType, AgriculturalEquipmentTypeDto>();

            CreateMap<CostUnit, CostUnitDto>();

            CreateMap<Crop, CropDto>();

            CreateMap<Fuel, FuelDto>();

            CreateMap<Producer, ProducerDto>();

            CreateMap<Seed, SeedDto>();

            CreateMap<Unit, UnitDto>();



            CreateMap<AgroControlUserRole,AgroControlUserFarmDto>();
            CreateMap<AgroControlRole, AgroControlRoleDto>();

            //Mapping from DTO
            CreateMap<ActiveIngredientDto, ActiveIngredient>();

            CreateMap<FarmDto, Farm>();

            CreateMap<AgroControlRoleDto, AgroControlRole>();
            CreateMap<AgroControlUserRoleDto, AgroControlUserRole>();
        }
    }
}
