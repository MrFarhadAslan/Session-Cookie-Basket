let deleteButtons = document.querySelectorAll(".deleteBtnImage");

deleteButtons.forEach(btn => btn.addEventListener("click", function () {
    btn.parentElement.remove()
}))

let itemDeleteBtns = document.querySelectorAll(".item-delete")

itemDeleteBtns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            let url = btn.getAttribute("href");
            fetch(url)
                .then(res => {
                    if (res.status == 200) {
                        window.location.reload(true)
                    }
                    else {
                        alert("Item tapilmadi")
                    }
                })
        }
    })
}))



//Add to basket

let addToBasketBtns = document.querySelectorAll(".add-to-basket-btn");

addToBasketBtns.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    let url = btn.getAttribute("href");

    fetch(url)
        .then(res => {
            if (res.status == 200) {
                alert("Add to basket")
                window.location.reload(true)
            }
            else {
                alert("Error!")
            }
        })
}))

//delete-item-to-basket

let deleteItem = document.querySelectorAll(".delete-item-to-basket");

deleteItem.forEach(btn => btn.addEventListener("click", function (e) {
    e.preventDefault();

    let url = btn.getAttribute("href");

    fetch(url)
        .then(res => {
            if (res.status == 200) {
                alert("DELETED!")
                window.location.reload(true)
            }
            else {
                alert("Error")
            }
        })
}))
