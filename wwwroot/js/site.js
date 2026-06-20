document.addEventListener("DOMContentLoaded", function () {
    console.log("NutriPlan JS cargado correctamente.");

    // --- BUSCADOR DE RECETAS ---
    const searchInput = document.getElementById("searchRecipe");
    if (searchInput) {
        searchInput.addEventListener("input", function () {
            const filter = searchInput.value.toLowerCase();
            const recipeTitles = document.querySelectorAll(".recipe-link");

            recipeTitles.forEach(title => {
                const text = title.textContent.toLowerCase();
                // Buscamos el contenedor de la columna para ocultar/mostrar la tarjeta completa
                const cardContainer = title.closest(".recipe-link-container");
                
                if (cardContainer) {
                    if (text.includes(filter)) {
                        cardContainer.style.display = ""; 
                        cardContainer.style.opacity = "1";
                    } else {
                        // En lugar de ocultar, difuminamos para que el usuario vea que hay filtro
                        cardContainer.style.opacity = "0.1"; 
                         
                    }
                }
            });
        });
    }

    // --- TARJETAS CLICABLES ---
    const cards = document.querySelectorAll('.clickable-card');
    cards.forEach(card => {
        card.addEventListener('click', function (e) {
            // Si el clic fue en un botón específico dentro de la tarjeta, dejamos que siga su curso
            if (e.target.tagName === 'BUTTON' || e.target.tagName === 'A') return;

            const url = this.getAttribute('data-url');
            if (url && url !== "#") {
                this.style.transform = "scale(0.97)";
                this.style.opacity = "0.8";
                this.style.transition = "all 0.2s ease";
                window.location.href = url;
            }
        });
    });

    // --- EFECTOS VISUALES ---
    
    const ctaBtn = document.querySelector('.btn-cta.btn-success');
    if (ctaBtn) {
        setInterval(() => {
            ctaBtn.classList.toggle('shadow-lg');
        }, 2000);
    }

    const fadeInSections = ['perfil-content', 'pills-tabContent'];
    fadeInSections.forEach(id => {
        const element = document.getElementById(id);
        if (element) {
            element.style.opacity = "0";
            element.style.transform = "vertical-align: bottom";
            setTimeout(() => {
                element.style.transition = "opacity 0.8s ease, transform 0.8s ease";
                element.style.opacity = "1";
            }, 100);
        }
    });

    // --- FUNCIONALIDAD LOGIN---
    const togglePass = document.getElementById('togglePassword');
    const passInput = document.getElementById('passwordInput');
    
    if (togglePass && passInput) {
        togglePass.addEventListener('click', function() {
            const type = passInput.getAttribute('type') === 'password' ? 'text' : 'password';
            passInput.setAttribute('type', type);
            const icon = this.querySelector('i');
            if (icon) {
                icon.classList.toggle('bi-eye');
                icon.classList.toggle('bi-eye-slash');
            }
        });
    }

    const loginCard = document.getElementById('login-container');
    if (loginCard) {
        loginCard.style.opacity = "0";
        loginCard.style.transform = "translateY(20px)";
        setTimeout(() => {
            loginCard.style.transition = "all 0.6s ease-out";
            loginCard.style.opacity = "1";
            loginCard.style.transform = "translateY(0)";
        }, 100);
    }

    // --- FUNCIONALIDAD REGISTRO ---
    const registerCard = document.getElementById('register-container');
    if (registerCard) {
        registerCard.style.opacity = "0";
        registerCard.style.transform = "scale(0.95)";
        setTimeout(() => {
            registerCard.style.transition = "all 0.5s ease-out";
            registerCard.style.opacity = "1";
            registerCard.style.transform = "scale(1)";
        }, 100);
    }
});