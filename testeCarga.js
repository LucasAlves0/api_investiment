import http from 'k6/http';
import { check, sleep } from 'k6';
import { Trend } from 'k6/metrics';

// Métricas personalizadas
let getProductTrend = new Trend('getProductDuration');
let getStatementTrend = new Trend('getStatementDuration');

export const options = {
  stages: [
    { duration: '30s', target: 100 }, // Subir para 100 usuários em 30 segundos
    { duration: '1m', target: 100 },  // Manter 100 usuários por 1 minuto
    { duration: '30s', target: 0 },   // Descer para 0 usuários em 30 segundos
  ],
  thresholds: {
    'http_req_duration{tag:product}': ['p(95)<100'], // 95% das requisições devem ser abaixo de 100ms
    'http_req_duration{tag:statement}': ['p(95)<100'],
  },
};

export default function () {
  // Testar o endpoint de produtos
  let productRes = http.get('http://localhost:5106/api/InvestmentProduct');
  check(productRes, {
    'status é 200': (r) => r.status === 200,
  });
  getProductTrend.add(productRes.timings.duration, { tag: 'product' });

  // Testar o endpoint de extrato do cliente
  let statementRes = http.get('http://localhost:5106/api/Transaction/statement/1'); // Use um ID de cliente válido
  check(statementRes, {
    'status é 200': (r) => r.status === 200,
  });
  getStatementTrend.add(statementRes.timings.duration, { tag: 'statement' });

  sleep(1);
}
