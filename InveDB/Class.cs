@{
    ViewData["Title"] = "Gestionar Tablas - Pizza Planet";
    var productos = ViewData["Productos"] as IEnumerable<dynamic>;
    var categorias = ViewData["Categorias"] as IEnumerable<InveDB.Modelos.Categoria>;
    var proveedores = ViewData["Proveedores"] as IEnumerable<InveDB.Modelos.Proveedor>;
    var sucursales = ViewData["Sucursales"] as IEnumerable<InveDB.Modelos.Sucursal>;
}

< !DOCTYPE html >
< html lang = "es" >
< head >
    < meta charset = "utf-8" />
    < title > Gestionar Tablas - Pizzería Planet </ title >
    < link href = "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel = "stylesheet" >
    < script src = "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js" ></ script >
    < style >
        body {
background: #f4f6f9;
        }

        .sidebar {
            background: #2c3e50;
            height: 100vh;
color: white;
padding - top: 20px;
position: fixed;
width: 220px;
        }

            .sidebar a
{
    color: white;
    text-decoration: none;
    display: block;
    padding: 10px 20px;
}

                .sidebar a:hover, .sidebar a.active {
                    background: #1abc9c;
                }

            .sidebar h3
{
    font-size: 1.5rem;
    text-align: center;
    margin-bottom: 20px;
}

        .content {
            margin-left: 220px;
padding: 20px;
        }

        .table - container {
background: white;
    border - radius: 10px;
padding: 20px;
    box - shadow: 0 2px 10px rgba(0,0,0,0.1);
    margin - top: 20px;
    overflow - x: auto;
}

        .table - productos {
width: 100 %;
    border - collapse: collapse;
    font - size: 0.95rem;
}

            .table - productos th, .table-productos td {
                border: 1px solid #ddd;
                padding: 10px;
text - align: left;
            }

            .table - productos th {
                background: #2c3e50;
                color: white;
text - transform: uppercase;
            }

            .table - productos tr: nth - child(even) {
background: #f4f6f9;
            }

            .table - productos tr: hover {
background: #e0f7fa;
                cursor: pointer;
}

        .action - buttons {
display: flex;
    justify - content: center;
gap: 15px;
    margin - top: 20px;
}

        .btn - sim {
border: none;
padding: 10px 20px;
    border - radius: 5px;
color: white;
    font - weight: bold;
cursor: pointer;
}

        .btn - add {
    background - color: #28a745;
        }

        .btn - edit {
    background - color: #ffc107;
            color: black;
}

        .btn - delete {
    background - color: #dc3545;
        }

# formPanel {
display: none;
background: white;
padding: 20px;
border - radius: 10px;
margin - top: 20px;
box - shadow: 0 - 2px 10px rgba(0,0,0,0.1);
        }

        .nav - tabs.nav - link.active {
    background - color: #1abc9c !important;
            color: white!important;
}
    </ style >
</ head >

< body >
    < div class= "sidebar" >
        < h3 > Pizza Planet 🪐</h3>
        <a asp-controller="Inicio" asp-action="Index">Inicio</a>
        <a asp-controller="Inventario" asp-action="Index">Inventario</a>
        <a class= "active" asp - controller = "GestionarTablas" asp - action = "Index" > Gestionar Tablas </ a >
        < a asp - controller = "EntradasSalidas" asp - action = "Index" > Entradas / Salidas </ a >
    </ div >

    < div class= "content" >
        < h3 class= "mb-4" > Gestor de Tablas</h3>

        <!-- Pestañas -->
        <ul class= "nav nav-tabs" id = "tabMenu" >
            < li class= "nav-item" >< button class= "nav-link active" data - bs - toggle = "tab" data - bs - target = "#tabProductos" > Productos </ button ></ li >
            < li class= "nav-item" >< button class= "nav-link" data - bs - toggle = "tab" data - bs - target = "#tabCategorias" > Categorías </ button ></ li >
            < li class= "nav-item" >< button class= "nav-link" data - bs - toggle = "tab" data - bs - target = "#tabProveedores" > Proveedores </ button ></ li >
            < li class= "nav-item" >< button class= "nav-link" data - bs - toggle = "tab" data - bs - target = "#tabSucursales" > Sucursales </ button ></ li >
        </ ul >

        < div class= "tab-content mt-4" >
            < !-- ====================== TAB PRODUCTOS ====================== -->
            < div class= "tab-pane fade show active" id = "tabProductos" >
                < div class= "table-container" >
                    < h4 > Productos </ h4 >
                    < table class= "table-productos" id = "tablaProductos" >
                        < thead >
                            < tr >
                                < th > ID </ th >
                                < th > Producto </ th >
                                < th > Categoría </ th >
                                < th > Precio Unitario </ th >
                                < th > Unidad Medida </ th >
                            </ tr >
                        </ thead >
                        < tbody >
                            @foreach(var p in productos)
                            {
                                < tr data - id = "@p.IdProducto" data - nombre = "@p.Nombre" data - categoria = "@p.Categoria" data - precio = "@p.PrecioUnitario" data - unidad = "@p.UnidadMedida" >
                                    < td > @p.IdProducto </ td >
                                    < td > @p.Nombre </ td >
                                    < td > @p.Categoria </ td >
                                    < td > $@p.PrecioUnitario </ td >
                                    < td > @p.UnidadMedida </ td >
                                </ tr >
                            }
                        </ tbody >
                    </ table >

                    < div class= "action-buttons" >
                        < button class= "btn-sim btn-add" id = "btnAdd" > Añadir </ button >
                        < button class= "btn-sim btn-edit" id = "btnEdit" disabled > Modificar </ button >
                        < button class= "btn-sim btn-delete" id = "btnDelete" disabled > Eliminar </ button >
                    </ div >

                    < div id = "formPanel" >
                        < form id = "productoForm" method = "post" >
                            < input type = "hidden" id = "IdProducto" name = "IdProducto" />
                            < div class= "row" >
                                < div class= "col-md-4" >
                                    < label > Nombre </ label >
                                    < input type = "text" class= "form-control" id = "Nombre" name = "Nombre" required />
                                </ div >
                                < div class= "col-md-4" >
                                    < label > Categoría </ label >
                                    < select class= "form-select" id = "IdCategoria" name = "IdCategoria" required >
                                        < option value = "" > Seleccionar categoría...</ option >
                                        @foreach(var c in categorias)
                                        {
                                            < option value = "@c.IdCategoria" > @c.Nombre </ option >
                                        }
                                    </ select >
                                </ div >
                                < div class= "col-md-4" >
                                    < label > Precio Unitario </ label >
                                    < input type = "number" step = "0.01" class= "form-control" id = "PrecioUnitario" name = "PrecioUnitario" required />
                                </ div >
                                < div class= "col-md-4 mt-3" >
                                    < label > Unidad de Medida</label>
                                    <input type = "text" class= "form-control" id = "UnidadMedida" name = "UnidadMedida" required />
                                </ div >
                            </ div >
                            < div class= "mt-3 text-end" >
                                < button type = "submit" class= "btn btn-success" id = "btnConfirmar" > Confirmar </ button >
                                < button type = "button" class= "btn btn-secondary" id = "btnCancelar" > Cancelar </ button >
                            </ div >
                        </ form >
                    </ div >
                </ div >
            </ div >

            < !-- ====================== TAB CATEGORÍAS ====================== -->
            < div class= "tab-pane fade" id = "tabCategorias" >
                < div class= "table-container" >
                    < h4 > Categorías </ h4 >
                    < table class= "table-productos" id = "tablaCategorias" >
                        < thead >
                            < tr >< th > ID </ th >< th > Nombre </ th >< th > Descripción </ th ></ tr >
                        </ thead >
                        < tbody >
                            @foreach(var c in categorias)
                            {
                                < tr data - id = "@c.IdCategoria" data - nombre = "@c.Nombre" data - descripcion = "@c.Descripcion" >
                                    < td > @c.IdCategoria </ td >
                                    < td > @c.Nombre </ td >
                                    < td > @c.Descripcion </ td >
                                </ tr >
                            }
                        </ tbody >
                    </ table >
                </ div >
            </ div >

            < !-- ====================== TAB PROVEEDORES ====================== -->
            < div class= "tab-pane fade" id = "tabProveedores" >
                < div class= "table-container" >
                    < h4 > Proveedores </ h4 >
                    < table class= "table-productos" id = "tablaProveedores" >
                        < thead >
                            < tr >< th > ID </ th >< th > Nombre </ th >< th > Teléfono </ th >< th > Dirección </ th ></ tr >
                        </ thead >
                        < tbody >
                            @foreach(var pr in proveedores)
                            {
                                < tr data - id = "@pr.IdProveedor" data - nombre = "@pr.Nombre" data - telefono = "@pr.Telefono" data - direccion = "@pr.Direccion" >
                                    < td > @pr.IdProveedor </ td >
                                    < td > @pr.Nombre </ td >
                                    < td > @pr.Telefono </ td >
                                    < td > @pr.Direccion </ td >
                                </ tr >
                            }
                        </ tbody >
                    </ table >
                </ div >
            </ div >

            < !-- ====================== TAB SUCURSALES ====================== -->
            < div class= "tab-pane fade" id = "tabSucursales" >
                < div class= "table-container" >
                    < h4 > Sucursales </ h4 >
                    < table class= "table-productos" id = "tablaSucursales" >
                        < thead >
                            < tr >< th > ID </ th >< th > Nombre </ th >< th > Dirección </ th >< th > Teléfono </ th ></ tr >
                        </ thead >
                        < tbody >
                            @foreach(var s in sucursales)
                            {
                                < tr data - id = "@s.IdSucursal" data - nombre = "@s.Nombre" data - direccion = "@s.Direccion" data - telefono = "@s.Telefono" >
                                    < td > @s.IdSucursal </ td >
                                    < td > @s.Nombre </ td >
                                    < td > @s.Direccion </ td >
                                    < td > @s.Telefono </ td >
                                </ tr >
                            }
                        </ tbody >
                    </ table >
                </ div >
            </ div >
        </ div >
    </ div >

    < script >
        // --- Tu script original intacto ---
        let modo = "";
let filaSeleccionada = null;

document.querySelectorAll("#tablaProductos tbody tr").forEach(fila => {
    fila.addEventListener("click", () => {
        document.querySelectorAll("#tablaProductos tr").forEach(f => f.classList.remove("table-primary"));
        fila.classList.add("table-primary");
        filaSeleccionada = fila;
        document.getElementById("btnEdit").disabled = false;
        document.getElementById("btnDelete").disabled = false;
    });
});

document.getElementById("btnAdd").addEventListener("click", () => {
    modo = "add";
    document.getElementById("formPanel").style.display = "block";
    document.getElementById("productoForm").action = "/GestionarTablas/AgregarProducto";
    document.getElementById("productoForm").reset();
});

document.getElementById("btnEdit").addEventListener("click", () => {
    if (!filaSeleccionada) return;
    modo = "edit";
    document.getElementById("formPanel").style.display = "block";
    document.getElementById("productoForm").action = "/GestionarTablas/EditarProducto";
    document.getElementById("IdProducto").value = filaSeleccionada.dataset.id;
    document.getElementById("Nombre").value = filaSeleccionada.dataset.nombre;
    document.getElementById("PrecioUnitario").value = filaSeleccionada.dataset.precio;
    document.getElementById("UnidadMedida").value = filaSeleccionada.dataset.unidad;
    const categoria = filaSeleccionada.dataset.categoria;
    const select = document.getElementById("IdCategoria");
    for (let opt of select.options) if (opt.text === categoria) opt.selected = true;
});

document.getElementById("btnDelete").addEventListener("click", () => {
    if (!filaSeleccionada) return;
    const id = filaSeleccionada.dataset.id;
    if (confirm("¿Seguro que deseas eliminar este producto?"))
    {
        const form = document.createElement("form");
        form.method = "post";
        form.action = "/GestionarTablas/EliminarProducto";
        const input = document.createElement("input");
        input.type = "hidden"; input.name = "id"; input.value = id;
        form.appendChild(input); document.body.appendChild(form); form.submit();
    }
});

document.getElementById("btnCancelar").addEventListener("click", () => {
    document.getElementById("formPanel").style.display = "none";
});
    </ script >
</ body >
</ html >
