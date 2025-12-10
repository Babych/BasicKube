# Basickube Deployment on AKS with TLS (nip.io)

This project demonstrates a simple Kubernetes deployment on AKS, exposed via an Ingress using **nip.io**, with automatic TLS certificates issued by **cert-manager** and Let's Encrypt (staging or production).

---

## Features

- Kubernetes Deployment & Service
- Ingress with nip.io domain
- TLS via cert-manager
- Optional self-signed certificates for local development
- GitHub Actions compatible for CI/CD deployment

---

## Prerequisites

- Azure Kubernetes Service (AKS) cluster
- `kubectl` configured for the cluster
- NGINX Ingress Controller installed
- cert-manager installed in the cluster

```bash
kubectl apply -f https://github.com/cert-manager/cert-manager/releases/latest/download/cert-manager.yaml
GitHub Actions runner (optional for CI/CD)

Deployment Steps
1. ClusterIssuer (Let's Encrypt staging)
Create a ClusterIssuer for staging certificates:

yaml
Copy code
apiVersion: cert-manager.io/v1
kind: ClusterIssuer
metadata:
  name: letsencrypt-staging
spec:
  acme:
    server: https://acme-staging-v02.api.letsencrypt.org/directory
    email: your-email@example.com
    privateKeySecretRef:
      name: letsencrypt-staging
    solvers:
    - http01:
        ingress:
          class: nginx
Apply it:

bash
Copy code
kubectl apply -f cluster-issuer.yaml
For production, use server: https://acme-v02.api.letsencrypt.org/directory and change the secret name.

2. Deployment & Service
Create your Deployment and Service for basickube:

yaml
Copy code
# deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: basickube
spec:
  replicas: 1
  selector:
    matchLabels:
      app: basickube
  template:
    metadata:
      labels:
        app: basickube
    spec:
      containers:
      - name: basickube
        image: your-image
        ports:
        - containerPort: 8080

# service.yaml
apiVersion: v1
kind: Service
metadata:
  name: basickube-service
spec:
  selector:
    app: basickube
  ports:
    - port: 80
      targetPort: 8080
  type: ClusterIP
Apply them:

bash
Copy code
kubectl apply -f deployment.yaml
kubectl apply -f service.yaml
3. Ingress with TLS (cert-manager)
yaml
Copy code
apiVersion: networking.k8s.io/v1
kind: Ingress
metadata:
  name: basickube-ingress
  namespace: basickube-ns
  annotations:
    cert-manager.io/cluster-issuer: letsencrypt-staging
spec:
  ingressClassName: nginx
  rules:
  - host: basickube.172.199.209.31.nip.io
    http:
      paths:
      - path: /
        pathType: Prefix
        backend:
          service:
            name: basickube-service
            port:
              number: 80
  tls:
  - hosts:
    - basickube.172.199.209.31.nip.io
    secretName: basickube-tls
Apply it:

bash
Copy code
kubectl apply -f basickube-ingress.yaml
Check certificate status:

bash
Copy code
kubectl describe certificate basickube-tls -n basickube-ns
kubectl get secret basickube-tls -n basickube-ns
4. Access the Application
HTTP: http://basickube.172.199.209.31.nip.io/weatherforecast

HTTPS (staging): https://basickube.172.199.209.31.nip.io/weatherforecast

Chrome may show "Not Secure" if using staging or self-signed certificate. Edge may display it as secure.

5. Optional: Self-signed certificate (local dev)
Generate a self-signed TLS certificate:

bash
Copy code
openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
  -keyout basickube.key \
  -out basickube.crt \
  -subj "/CN=basickube.172.199.209.31.nip.io"

kubectl create secret tls basickube-tls \
  --cert=basickube.crt \
  --key=basickube.key \
  -n basickube-ns
Update Ingress to use basickube-tls secret. Optionally, add the CA to your system/browser to remove warnings.

6. CI/CD (GitHub Actions example)
You can add a workflow that deploys Deployment + Service + Ingress:

yaml
Copy code
on:
  push:
    branches:
      - main
  workflow_dispatch:
    inputs:
      branch:
        description: 'Branch to deploy'
        required: true
        default: main

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
        with:
          ref: ${{ github.event.inputs.branch || github.ref_name }}

      - name: Deploy to AKS
        run: |
          kubectl apply -f k8s/deployment.yaml
          kubectl apply -f k8s/service.yaml
          kubectl apply -f k8s/basickube-ingress.yaml
Notes
Staging certificates are for testing — browser may warn.

Production certificates are limited (50 per week per domain for Let’s Encrypt).

nip.io allows quick DNS for IPs without manual DNS configuration.

For fully trusted HTTPS on Chrome with self-signed certificates, add your CA to Trusted Root.