# Vercel + DigitalOcean Setup Checklist

Practical checklist to execute Sprint 0 for hosted deployment.

## 1) Vercel Project Setup (Web)

- [ ] Import `SGC.Web/sgc.web.client/sgc-medagenda` as a Vercel project.
- [ ] Set framework preset to Next.js.
- [ ] Configure env vars:
  - [ ] `NEXT_PUBLIC_API_URL`
  - [ ] `NEXT_PUBLIC_API_BASE_URL` (optional legacy fallback)
- [ ] Enable preview deployments.
- [ ] Bind staging and production domains.

## 2) DigitalOcean API Setup

Choose one runtime:

- [ ] Option A: App Platform (recommended for faster setup)
- [ ] Option B: Droplet + Docker Compose (more control)

Then:

- [ ] Deploy API from repo root using `SGC.API/Dockerfile`.
- [ ] Inject required API env vars (JWT, DB, Redis, Stripe, AI, CORS).
- [ ] Expose `/health` endpoint publicly or through trusted network.
- [ ] Verify API startup succeeds with non-dev CORS configuration.

## 3) Managed Services

- [ ] Provision PostgreSQL (DO managed or external).
- [ ] Provision Redis (DO managed or external).
- [ ] Validate connectivity from API runtime.

## 4) Domain + TLS + CORS

- [ ] Point `api-staging.<domain>` and `api.<domain>` to DO runtime.
- [ ] Ensure HTTPS certificates are active.
- [ ] Set `Cors__AllowedOrigins__0/1` for Vercel domains.
- [ ] Confirm preflight requests succeed from browser.

## 5) Smoke Validation

- [ ] Web loads and can call API.
- [ ] Login works.
- [ ] Patient citas list works.
- [ ] SignalR connection to `/citahub` works.
- [ ] Payment intent creation endpoint works.

## 6) Release Gates

- [ ] `dotnet build SGC.sln` passes.
- [ ] `dotnet test SGC.ApplicationTest/SGC.ApplicationTest.csproj` passes.
- [ ] `npm run lint` passes.
- [ ] Deployment notes updated in README/backlog docs.
