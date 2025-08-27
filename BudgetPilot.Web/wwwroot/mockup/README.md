# Dashboard Mockup - BudgetPilot

Mockup della dashboard seguendo la struttura del file di riferimento fornito.

## Contenuti

- **index.html**: Dashboard completa con design responsive
- **mockup.js**: Inizializzazione grafico 50/30/20 e dark mode toggle
- **Struttura**: Basata sul mockup di riferimento SmartPFM

## Componenti Implementati

### KPI Cards
- Reddito Netto Mensile: €3,000
- Spese Totali: €2,850  
- Risparmio Effettivo: €150 (5%)

### Grafico 50/30/20
- Doughnut chart con Chart.js
- Visualizzazione percentuali: Necessità 58%, Desideri 27%, Risparmi 10%
- Responsive e con supporto dark mode

### Tabella Transazioni
- Ultime 5 transazioni con dettagli
- Categorizzazione per necessità/desideri/risparmi/entrate
- Design responsive con hover effects

## Componenti Blazor Identificati

Per l'implementazione futura in Blazor:

1. **KpiCard** 
   - Props: titolo, valore, descrizione, trend
   
2. **BudgetAnalysisCard**
   - Props: categoria, importo attuale, target, percentuale, items
   - Colori: red (necessità), yellow (desideri), green (risparmi)
   
3. **FiftyThirtyTwentyChart**
   - Props: valori necessità, desideri, risparmi
   - Chart.js integration
   
4. **TransactionsTable**
   - Props: lista transazioni, colonne configurabili
   - Supporto paginazione e filtri

5. **DashboardLayout** 
   - Container principale con navigation
   - Dark mode support
   - Responsive grid layout

## Features

- ✅ Design responsive con Tailwind CSS
- ✅ Dark mode toggle funzionante
- ✅ Grafico 50/30/20 interattivo
- ✅ KPI cards con dati mockup
- ✅ Tabella transazioni responsive
- ✅ Animazioni e transizioni
- ✅ Accessibile su `/mockup`

## Design System

- **Colors**: Blue brand, red (necessità), yellow (desideri), green (risparmi)
- **Typography**: Tailwind font scales
- **Icons**: Lucide React icons
- **Layout**: CSS Grid e Flexbox
- **Animations**: CSS keyframes per slide-in effects
