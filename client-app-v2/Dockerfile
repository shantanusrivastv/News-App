# Stage 1: Build the React app
FROM node:20-alpine AS build
# Set the working directory
WORKDIR /app
# Copy the package.json and package-lock.json files
COPY package*.json ./
# Install the dependencies
RUN yarn
# Copy the app files
COPY . .
# Build the app
RUN yarn run build

# Stage 2: Serve the React app using Nginx
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]