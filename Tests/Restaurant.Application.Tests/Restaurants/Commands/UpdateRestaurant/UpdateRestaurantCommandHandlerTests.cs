using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Resturants.Application.Restaurants.Commands.UpdateRestaurant;
using Resturants.Application.Restaurants.Dtos;
using Resturants.Domain.Entities;
using Resturants.Domain.Enums;
using Resturants.Domain.Exceptions;
using Resturants.Domain.Interfaces.Repositories;
using Resturants.Domain.Interfaces.Services;
using System.Linq.Expressions;

namespace Resturants.Application.Tests.Restaurants.Commands.UpdateRestaurant;

[TestFixture]
public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock;

    private Mock<IGenericRepository<Restaurant>> _restaurantRepo;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    private Mock<IRestaurantAuthoriazationService> _restaurantAuthoriazationServiceMock;

    private UpdateRestaurantCommandHandler _cut;

    public UpdateRestaurantCommandHandlerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();  
    }

    [SetUp]
    public void SetUp()
    {
        _restaurantRepo = new Mock<IGenericRepository<Restaurant>>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _restaurantAuthoriazationServiceMock = new Mock<IRestaurantAuthoriazationService>();

        _cut = new UpdateRestaurantCommandHandler(_mapperMock.Object, _unitOfWorkMock.Object, _loggerMock.Object, _restaurantAuthoriazationServiceMock.Object);
    }

    [Test]
    public async Task Handle_IfValidRequest_ShallUpdateRestaurant()
    {
        var restaruantId = 1;
        var command = new UpdateRestaurantCommand()
        {
            Name = "New Test",
            Description = "New Description",
            HasDelivery = true,
        };
        command.SetId(restaruantId);

        var restaurant = new Restaurant()
        {
            Id = restaruantId,
            Name = "Test",
            Description = "Test",
            HasDelivery = false,
        };

        _unitOfWorkMock.Setup(u => u.Resturants).Returns(_restaurantRepo.Object);

        _restaurantRepo.Setup(r => r.FindAsync(It.Is<Expression<Func<Restaurant, bool>>>(expr => expr.Compile().Invoke(restaurant)), null))
            .ReturnsAsync(restaurant);

        _restaurantAuthoriazationServiceMock.Setup(a => a.Authorize(restaurant, CrudOperations.update))
        .Returns(true);



        await _cut.Handle(command, CancellationToken.None);



        _mapperMock.Verify(m => m.Map(command, restaurant), Times.Once());
        _restaurantRepo.Verify(r => r.Update(restaurant), Times.Once());
        _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once());
        _mapperMock.Verify(m => m.Map<RestaurantDto>(restaurant), Times.Once());
    }


    [Test]
    public async Task Handle_IfNotExistingRestaurant_ShallThrowNotFoundException()
    {
        var command = new UpdateRestaurantCommand();
        command.SetId(0);

        var restaurant = new Restaurant();

        _unitOfWorkMock.Setup(u => u.Resturants).Returns(_restaurantRepo.Object);

        _restaurantRepo.Setup(r => r.FindAsync(It.Is<Expression<Func<Restaurant, bool>>>(expr => expr.Compile().Invoke(restaurant)), null))
          .ReturnsAsync((Restaurant)null);

        _restaurantAuthoriazationServiceMock.Setup(a => a.Authorize(restaurant, CrudOperations.update))
          .Returns(false);



        Assert.ThrowsAsync<NotFoundException>(async () => await _cut.Handle(command, CancellationToken.None));
    }
    [Test]
    public async Task Handle_IfNotAuthorizedUser_ShallThrowForbiddenException()
    {
        var command = new UpdateRestaurantCommand();
        command.SetId(0);

        var restaurant = new Restaurant();

        _unitOfWorkMock.Setup(u => u.Resturants).Returns(_restaurantRepo.Object);

        _restaurantRepo.Setup(r => r.FindAsync(It.Is<Expression<Func<Restaurant, bool>>>(expr => expr.Compile().Invoke(restaurant)), null))
          .ReturnsAsync(restaurant);



        Assert.ThrowsAsync<ForbiddenException>(async () => await _cut.Handle(command, CancellationToken.None));
    }
}
