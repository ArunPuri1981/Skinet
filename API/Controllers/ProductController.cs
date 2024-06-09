using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using API.DTO;
using AutoMapper;
using API.Error;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductRepository _productRepository; 
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper;                        
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDTO>>> GetProduct()
        {
            var products=await _productRepository.GetProductsAsync();
            return Ok(_mapper.Map<IReadOnlyList<Product>,IReadOnlyList<ProductToReturnDTO>>(products));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]//Swagger
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]//Swagger
        public async Task<ActionResult<ProductToReturnDTO>> GetProductById(int id)
        {
            var product= await _productRepository.GetProductByIdAsync(id);

            if(product==null)
            {
                return NotFound(new ApiResponse(400));
            }    

            return Ok(_mapper.Map<Product,ProductToReturnDTO>(product));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>>GetProductTypes()
        {
            return Ok(await _productRepository.GetProductTypesAsync());
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>>GetProductBrands()
        {
            return Ok(await _productRepository.GetProductBrandsAsync());
        }
    }
}