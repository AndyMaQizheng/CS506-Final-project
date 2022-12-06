var shopping = {
    products: [
        {
            id: 1,
            name: 'Samsung-75" Class TV',
            price: 569.99,
            image:
                'https://pisces.bbystatic.com/image2/BestBuy_US/images/products/6514/6514052_sd.jpg',
            inCart: 0,
        },
        {
            id: 2,
            name: 'Hisense - 55" Class TV',
            price: 235.98,
            image:
                'https://pisces.bbystatic.com/image2/BestBuy_US/images/products/6492/6492224_sd.jpg',
            inCart: 0,
        },
        {
            id: 3,
            name: 'Samsung-FHD HDR Smart Projector-White',
            price: 499,
            image:
                'https://pisces.bbystatic.com/image2/BestBuy_US/images/products/6492/6492951_sd.jpg',
            inCart: 0,
        },
        {
            id: 4,
            name: 'TCL-50" Class Roku TV',
            price: 239.99,
            image:
                'https://pisces.bbystatic.com/image2/BestBuy_US/images/products/6514/6514455_sd.jpg',
            inCart: 0,
        },
        {
            id: 5,
            name: 'Lenovo-15.6" HD Touch Laptop-Core i3-1115G4-8GB Memory-256GB SSD-Platinum Grey',
            price: 245.96,
            image:
                'https://pisces.bbystatic.com/image2/BestBuy_US/images/products/6511/6511950_sd.jpg',
            inCart: 0,
        },
        {
            id: 6,
            name: 'Great Value Purified Drinking Water, ' +
                '16.9 Fl Oz, 40 Count Bottles',
            price: 5.36,
            image:
                'https://i5.walmartimages.com/asr/90d2154f-9b8e-4aa8-b034-6e68c31bb3e1.55befbec8647c217b460869858db9c4b.jpeg',
            inCart: 0,
        },
        {
            id: 7,
            name: 'Ground Beef, Chuck',
            price: 5.58,
            image:
                'https://i5.walmartimages.com/asr/becdc204-72d5-43a7-82e4-dc4ecc751c46.6abc698327fdda741d04c20f1457ea88.jpeg',
            inCart: 0,
        },
        {
            id: 8,
            name: 'Nike bag',
            price: 81,
            image:
                'https://static.nike.com/a/images/c_limit,w_592,f_auto/t_product_v1/oixxif9s2hpi8798pfy3/tanjun-backpack-FC8qMk.jpg',
            inCart: 0,
        },
        {
            id: 9,
            name: 'White Sandwich Bread',
            price: 1.5,
            image:
                'https://i5.walmartimages.com/asr/0c83dbaf-9ef7-46b1-bde6-7ee02ad8c1f8.b18788ee71ec44f0b188c06c2f923b09.jpeg',
            inCart: 0,
        },
        {
            id: 10,
            name: 'Port Authority Man',
            price: 21.7,
            image:
                'https://i5.walmartimages.com/asr/857c99e8-7212-40d0-87e8-2018982ebe0f_1.0c31a671dd8ccff16537cdb537e7d607.jpeg',
            inCart: 0,
        },
        {
            id: 11,
            name: 'Double Breasted Coat',
            price: 69,
            image:
                'https://i5.walmartimages.com/asr/2d052690-f28c-4974-910c-463dd1698253.c44c7dc25f7a7b5cd493c8900ba559c5.jpeg',
            inCart: 0,
        },
        {
            id: 12,
            name: 'Plush Sleep Pants',
            price: 10,
            image:
                'https://i5.walmartimages.com/asr/75d0d5f5-36fd-4556-9702-c4a0406b0b37.d1ea0411eea98e1c093f469022171f91.jpeg',
            inCart: 0,
        },
        {
            id: 13,
            name: 'Gretchen Shearling Hiker Boot',
            price: 45,
            image:
                'https://i5.walmartimages.com/asr/655de5fa-c1c0-479a-98ff-d200a93abdba.256bbc36b69e9cfff154a899b4b0e486.jpeg',
            inCart: 0,
        },

    ],
    max : 25,

    init() {
        this.renderProduct();
        this.addEvent();
        this.initLocalStorage();
        this.renderCart();
    },

    initLocalStorage() {
        if(!localStorage.getItem('productInCart')){
            let productInCart =[];
            for(item of this.products){
                if(item.inCart > 0){
                    productInCart.push({id: `${item.id}`, inCart: `${item.inCart}`});
                }
            }
            localStorage.setItem('productInCart', JSON.stringify(productInCart));
        }else{
            let productList = JSON.parse(localStorage.getItem('productInCart'));
            console.log(productList);
            for(item of productList){
                index = this.getProductIndexById(parseInt(item.id));
                this.products[index].inCart = parseInt(item.inCart);
            }
            let totalPrice = 0;
            let tax=0;
            let total=0
            for(item of this.products){
                if(item.inCart > 0){
                    totalPrice += item.inCart * item.price
                    tax= totalPrice * 0.15
                    total=tax+totalPrice
                }
            }
            this.loadCart(totalPrice,tax,total);
        }
    },

    addEvent() {
        $('.cart').click(function(){
            $('.table-cart').removeClass('display');
        });
        $('.add-to-cart').click(function(){
            let item = shopping.getProduct($(this).attr('id'));
            if(item.inCart == 0){
                let productList = JSON.parse(localStorage.getItem('productInCart'));
                console.log(productList);

                productList.push({id: `${item.id}`, inCart: 1});

                localStorage.setItem('productInCart', JSON.stringify(productList));
                shopping.products[`${item.id - 1}`].inCart = 1;

                let total = parseFloat(localStorage.getItem('totalPrice')) + item.price ;
                let tax = parseFloat(localStorage.getItem('tax')) + item.price*0.15 ;
                let totalp = parseFloat(localStorage.getItem('total')) + item.price+item.price*0.15 ;
                shopping.loadCart(total,tax,totalp);
                console.log(localStorage);

            }else if(item.inCart < shopping.max){
                shopping.changeProductQuantity($(this).attr('id'));
            }else{
                alert('This product quantity is maximum!');
            }
        })
    },

    renderProduct() {
        for (item of this.products) {
            let divProdItem = $('<div></div>').addClass('product-item'),
                divProdImage = $('<div></div>').addClass('product-img'),
                imgProdImage = $('<img>').attr({
                    src: `${item.image}`,
                    alt: `${item.name}`
                }),
                btnAddToCart = $('<button></button>').text('Add To Cart').addClass('add-to-cart').attr('id', `${item.id}`),
                divProdInfo = $('<div></div>').addClass('product-info'),
                h3ProdName = $('<h3></h3>')
                    .addClass('product-name')
                    .text(`${item.name}`),
                pProdPrice = $('<p></p>')
                    .addClass('product-price')
                    .text(`$${item.price.toFixed(2)}`);

            $('.product-list').append(
                divProdItem
                    .append(divProdImage.append(imgProdImage))
                    .append(divProdInfo.append(h3ProdName).append(pProdPrice))
                    .append(btnAddToCart)
            );
        }
    },

    changeProductQuantity(id, quantity = 1){
        let item = this.getProduct(id),
            totalPrice = localStorage.getItem('totalPrice'),tax=localStorage.getItem('tax'),totalp=localStorage.getItem('total');

        total1 = parseFloat(totalPrice) + item.price * quantity;
        tax1=parseFloat(tax)+item.price * quantity*0.15;
        total2=parseFloat(totalp)+item.price * quantity+item.price * quantity*0.15;
        item.inCart += quantity;
        if(item.inCart == 0){
            // bo product ra localStorage
            let productList = JSON.parse(localStorage.getItem('productInCart'));
            for(let i =0; i<productList.length;i++){
                if(productList[i].id == id){
                    productList.splice(i,1);
                    console.log(productList);
                    localStorage.setItem('productInCart', JSON.stringify(productList));
                    break;
                }
            }
            console.log(localStorage.getItem('productInCart'));
        }
        this.products[id -1].inCart = item.inCart;

        this.loadCart(total1.toFixed(2),tax1.toFixed(2),total2.toFixed(2));
    },

    loadCart(totalPrice=0,tax=0,total=0){
        console.log('local price' + localStorage.getItem('totalPrice'));
        localStorage.setItem('totalPrice', totalPrice);
        console.log('tax' + localStorage.getItem('tax'));
        localStorage.setItem('tax', tax);
        console.log('total price' + localStorage.getItem('total'));
        localStorage.setItem('total', total);

        let countCart= JSON.parse(localStorage.getItem('productInCart')).length;
        localStorage.setItem('countCart', countCart);

        let count = $('.count');
        count.empty();
        if(localStorage.getItem('countCart') > 100){
            count.text('99+');
        }else{
            count.text(localStorage.getItem('countCart'));
        }
        this.renderCart();
    },

    renderCart() {
        $(".product-in-cart").empty();
        for (item of this.products) {
            if (item.inCart > 0) {
                let row = $('<tr></tr>'),
                    colName = $('<td></td>').text(`${item.name}`),
                    colImg = $('<td></td>'),
                    prodImg = $('<img>').attr({src: `${item.image}`}),
                    colPrice = $('<td></td').text(`$${item.price.toFixed(2)}`),
                    colQuantity = $('<td></td>'),
                    colTotal = $('<td></td>').text(`$${(item.inCart*item.price).toFixed(2)}`),
                    inputQuantity = $('<input>').attr({type: 'number', value: `${item.inCart}`, min: '1'}),
                    btnIncrease = $('<button></button>').text('+').addClass('btn btn-increase').attr('id', `${item.id}`),
                    btnDecrease = $('<button></button>').text('-').addClass('btn btn-decrease').attr('id', `${item.id}`),
                    colDelete = $('<td></td>'),
                    btnDelete = $('<button></button>').text('x').addClass('btn btn-delete').attr('id', `${item.id}`);
                $('.product-in-cart').append(
                    row.append(colName)
                        .append(colImg.append(prodImg))
                        .append(colPrice)
                        .append(colQuantity.append(btnDecrease).append(inputQuantity).append(btnIncrease))
                        .append(colTotal)
                        .append(colDelete.append(btnDelete))
                );
            }
        }

        $('.price').text('$' + parseFloat(localStorage.getItem('totalPrice')).toFixed(2) );
        $('.Tax').text('$' +   parseFloat(localStorage.getItem('tax')).toFixed(2));
        $('.total-price').text('$' + parseFloat(localStorage.getItem('total')).toFixed(2));
        this.addEventToCard();
    },

    addEventToCard(){
        $('.btn-decrease').click(function(){
            let product = shopping.getProduct($(this).attr('id'));
            if (product.inCart > 1){
                shopping.changeProductQuantity($(this).attr('id'), -1);
                //shopping.renderCart();
            }
        });
        $('.btn-increase').click(function(){
            let product = shopping.getProduct($(this).attr('id'));
            if (product.inCart < shopping.max){
                shopping.changeProductQuantity($(this).attr('id'));
                // shopping.renderCart();
            }else{
                alert('product quantity is maximum~');
            }
        });
        $('.btn-delete').click(function(){
            if(confirm('Do you want to delete this item?')){
                let product = shopping.getProduct($(this).attr('id'));
                shopping.changeProductQuantity($(this).attr('id'), -(product.inCart))
                // shopping.renderCart();
            }
        });
    },

    getProduct(id){
        for(item of this.products)
            if(item.id == id) return item;
    },

    getProductIndexById(id){
        for(let i =0; i< this.products.length; i++){
            if(this.products[i].id == id){
                return i;
            }
        }
        return -1;
    }
};

$(function () {
    shopping.init();
});
