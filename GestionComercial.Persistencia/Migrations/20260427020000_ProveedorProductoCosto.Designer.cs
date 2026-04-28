using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using GestionComercial.Persistencia.Contexto;

#nullable disable

namespace GestionComercial.Persistencia.Migrations
{
    [DbContext(typeof(GestionComercialContext))]
    [Migration("20260427020000_ProveedorProductoCosto")]
    partial class ProveedorProductoCosto
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");
#pragma warning restore 612, 618
        }
    }
}