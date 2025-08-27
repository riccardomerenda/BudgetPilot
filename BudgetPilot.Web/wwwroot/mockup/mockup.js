// Initialize
document.addEventListener('DOMContentLoaded', function() {
    lucide.createIcons();
    initializeChart();
});

// Dark mode toggle
function toggleDarkMode() {
    document.body.classList.toggle('dark');
    updateChart();
}

// Chart Management
let budgetChart;

function initializeChart() {
    const ctx = document.getElementById('budget-chart');
    if (!ctx) return;
    
    budgetChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ['Necessità 58%', 'Desideri 27%', 'Risparmi 10%', 'Disponibile 5%'],
            datasets: [{
                data: [58, 27, 10, 5],
                backgroundColor: [
                    'rgb(239, 68, 68)',   // red-500
                    'rgb(245, 158, 11)',  // yellow-500  
                    'rgb(34, 197, 94)',   // green-500
                    'rgb(156, 163, 175)'  // gray-400
                ],
                borderWidth: 0,
                hoverOffset: 8
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom',
                    labels: {
                        padding: 20,
                        usePointStyle: true,
                        color: document.body.classList.contains('dark') ? '#e2e8f0' : '#1f2937'
                    }
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            return context.label + ': €' + (context.parsed * 30).toFixed(0);
                        }
                    }
                }
            },
            cutout: '60%'
        }
    });
}

function updateChart() {
    if (budgetChart) {
        budgetChart.options.plugins.legend.labels.color = 
            document.body.classList.contains('dark') ? '#e2e8f0' : '#1f2937';
        budgetChart.update();
    }
}

// Initialize when page loads
window.addEventListener('load', () => {
    lucide.createIcons();
    initializeChart();
});
