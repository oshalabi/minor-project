# Stage 1: Build the Angular app
FROM node:20.18.1-alpine3.20 AS build

# Set working directory
WORKDIR /app

# Copy package.json and install dependencies
COPY package*.json ./
RUN npm install --include=optional

# Copy the app source code and build
COPY . .
RUN npm run build:prod

# Stage 2: Serve the app with Nginx
FROM nginx:1.25-alpine

# Create necessary directories with appropriate permissions
RUN mkdir -p /var/cache/nginx/client_temp && \
    chown -R 1000:1000 /var/cache/nginx

# Copy the built Angular app to Nginx's default public directory
COPY --from=build /app/dist/web-app/browser /usr/share/nginx/html

COPY nginx.conf /etc/nginx/nginx.conf
# Copy the custom Nginx configuration
COPY nginx-default.conf /etc/nginx/conf.d/default.conf

# Change ownership of HTML files to match the non-root user
RUN chown -R 1000:1000 /usr/share/nginx/html

# Expose port 8080
EXPOSE 8080

# Set user explicitly
USER 1000

# Start Nginx
CMD ["nginx", "-g", "daemon off;"]
