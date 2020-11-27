## The problem
When you're developing a one-time tool or just something which you're never going to modify and touch (essentially meaning that it's a small project), you probably shouldn't follow any of the best practices simply because it's an overhead and overengineering. 

However if you're developing an enterprice level application which should be scallable, maintainable and extendable you should follow the common practices like SOLID principles.

### ProductApplicationService.cs
There the following main problems with this class:

1. <b>Open-Closed principle violation</b>: this class must be modified if you want to extend it with a new service.
2. <b>Single-Responsibility principle violation</b>: this class decides which service to call, how to create the input and how to process the output.
3. <b>Growing constructor</b>: each time you want to add a new service you need to modify the constructor. Although you probably will use DI to create an instance of this class, but you still need to update all the related tests.
4. <b>Code duplication</b>: there duplications of the ```CompanyDataRequest``` class initialization.
5. <b>Empty error</b>: if no result has been returned the method throws an empty error which is confusing. The error should have an error message.

## Solution
The solution for this problems will increase the amount of code twice, but it totally worths it: making an introducing of a new third party service very easy.

I decided to replace ```if{...}else{...}``` blocks with polymorphysm using the <b>Strategy</b> and <b>Factory</b> design patterns, this addresses the 1, 2 an 3 problems. Please see my comments in the ```IProductApplicationServiceFactory.cs```, ```ProductApplicationStrategyAbstract.cs``` and ```ApplicationResultStrategyAbstract.cs``` files.

To fix the 4th problem I created an extension method for ```ISellerCompanyData``` to map it to ```CompanyDataRequest```, but I want to emphasise that normally I <b>wouldn't do it that way</b>, instead <b>I would use AutoMapper</b> which brings more flexability and is easy to configure. For this particular case I decided it's an overhead. 

To address the 5th problem I used a simple method which builds an error message and avoids code duplication (see ```ProductApplicationStrategyAbstract.GetErrorMessage()```).