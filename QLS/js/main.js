document.addEventListener('DOMContentLoaded', function () { 
    fetchProducts(); 
    document.getElementById('btnAdd').addEventListener('click',addProduct); 
    document.getElementById('btnReset').addEventListener('click', resetForm); 

    document.getElementById('bookList').addEventListener('click', function (e) { 
        const target = e.target; 
        const id = target.getAttribute('data-id'); 
        if (target.classList.contains('delete-btn')) { 
            deleteProduct(id); 
        } else if (target.classList.contains('edit-btn')) { 
            loadProductToForm(id); 
        } else if (target.classList.contains('view-btn')) { 
            viewProductDetail(id); 
        } 
    }); 

    document.getElementById('btnUpdate').addEventListener('click', function (e) { 
        e.preventDefault(); 
        updateProduct(); 
    }); 
}); 
 
function fetchProducts() { 
    const apiUrl = 'http://localhost:5189/api/products'; 
    fetch(apiUrl) 
    .then(handleResponse) 
    .then(data => displayProducts(data)) 
    .catch(error => console.error('Fetch error:', 
    error.message)); 
}

function handleResponse(response) { 
    if (!response.ok) throw new Error('Network response was not ok'); 
    return response.json(); 
} 
 
// Display products in the HTML table 
function displayProducts(products) { 
    const bookList = document.getElementById('bookList'); 
    bookList.innerHTML = ''; // Clear existing products 
    products.forEach(product => { 
    bookList.innerHTML += createProductRow(product); 
    }); 
} 
 
// Create HTML table row for a product 
function createProductRow(product) { 
 return ` 
        <tr> 
            <td>${product.id}</td> 
            <td>${product.name}</td> 
            <td>${product.price}</td> 
            <td>${product.description}</td> 
            <td> 
                <button class="btn btn-danger delete-btn" data-id="${product.id}">Delete</button> 
  <button class="btn btn-warning edit-btn" data-id="${product.id}">Edit</button> 
  <button class="btn btn-primary view-btn" data-id="${product.id}">View</button> 
   </td> 
        </tr> 
    `; 
} 
 
// Add a new product 
function addProduct() { 
    const productData = { 
    name: document.getElementById('bookName').value, 
    price: document.getElementById('price').value, 
    description: document.getElementById('description').value, 
    }; 
    
    fetch('http://localhost:5189/api/products', { 
    method: 'POST', 
    headers: { 'Content-Type': 'application/json' }, 
    body: JSON.stringify(productData), 
    })  
    .then(handleResponse) 
    .then(data => { 
    console.log('Product added:', data); 
    fetchProducts(); // Refresh the product list 
    resetForm();
    }) 
    .catch(error => console.error('Error:', error)); 
}

function deleteProduct(id) { 
    if (!confirm('Bạn có chắc chắn muốn xoá sản phẩm này?')) return; 
    fetch(`http://localhost:5189/api/products/${id}`, { 
    method: 'DELETE', 
    }) 
        .then(response => { 
            if (response.status === 204) { 
                fetchProducts(); 
            } else { 
                alert('Xoá thất bại!'); 
            } 
        }) 
        .catch(error => console.error('Error:', error)); 
}

function loadProductToForm(id) { 
    fetch(`http://localhost:5189/api/products/${id}`) 
        .then(handleResponse) 
        .then(product => { 
            document.getElementById('bookName').value = product.name; 
            document.getElementById('price').value = product.price; 
            document.getElementById('description').value = product.description; 
            document.getElementById('btnAdd').style.display = 'none'; 
            document.getElementById('btnUpdate').style.display = 'inline-block'; 
            document.getElementById('btnUpdate').setAttribute('data-id', product.id); 
        }) 
        .catch(error => console.error('Error:', error)); 
}

function updateProduct() { 
    const id = document.getElementById('btnUpdate').getAttribute('data-id'); 
    const updatedProduct = { 
        id: id, 
        name: document.getElementById('bookName').value, 
        price: document.getElementById('price').value, 
        description: document.getElementById('description').value, 
    }; 

    fetch(`http://localhost:5189/api/products/${id}`, { 
        method: 'PUT', 
        headers: { 'Content-Type': 'application/json' }, 
        body: JSON.stringify(updatedProduct), 
    }) 
        .then(response => { 
            if (response.status === 204) { 
                fetchProducts(); 
                resetForm(); 
            } else { 
                alert('Cập nhật thất bại!'); 
            } 
        }) 
        .catch(error => console.error('Error:', error)); 
}

function viewProductDetail(id) { 
    fetch(`http://localhost:5189/api/products/${id}`) 
        .then(handleResponse) 
        .then(product => { 
            document.querySelector('[data-atr="id"]').textContent = product.id; 
            document.querySelectorAll('[data-atr="bookname"]').forEach(el => el.textContent = product.name); 
            document.querySelector('[data-atr="description"]').textContent = product.description; 
            var modal = new bootstrap.Modal(document.getElementById('modalViewDetailInfo')); 
            modal.show(); 
        }) 
        .catch(error => console.error('Error:', error)); 
}

function resetForm(e) { 
    if (e) e.preventDefault(); 
    document.getElementById('studentForm').reset(); 
    document.getElementById('btnAdd').style.display = 'inline-block'; 
    document.getElementById('btnUpdate').style.display = 'none'; 
    document.getElementById('btnUpdate').removeAttribute('data-id'); 
}