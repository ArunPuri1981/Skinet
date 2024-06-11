using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using API.DTO;
using AutoMapper;
using API.Error;
using Core.Specification;
using API.Helpers;

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
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProduct([FromQuery] ProductSpecPrams productSpecPrams)
        {
            var  totalCount= await _productRepository.CountAsync(productSpecPrams);

            var products = await _productRepository.GetProductsAsync(productSpecPrams);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);

            var pageination = new Pagination<ProductToReturnDTO>(productSpecPrams.PageIndex,
                productSpecPrams.PageSize, totalCount ,data);

            return Ok(pageination);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]//Swagger
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]//Swagger
        public async Task<ActionResult<ProductToReturnDTO>> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound(new ApiResponse(400));
            }

            return Ok(_mapper.Map<Product, ProductToReturnDTO>(product));
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productRepository.GetProductTypesAsync());
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productRepository.GetProductBrandsAsync());
        }
    }
}