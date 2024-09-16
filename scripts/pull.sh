#!/bin/bash
ENV_ALIAS='ppr'

INTERPROCESS_API_ALIAS='sw.interprocess.api'
AUTH_API_ALIAS='sw.auth.api'
ASSET_API_ALIAS='sw.asset.api'
ROUTING_API_ALIAS='sw.routing.api'
ADMIN_API_ALIAS='sw.admin.api'
ONBOARDING_API_ALIAS='sw.onboarding.api'
GATEWAY_API_ALIAS='sw.gw'

UI_ALIAS='sw.ui'

IMAGE_TAG='latest'

export AZ_RC_NAME_ID=
export AZ_RC_ACCESS_USER_ID=
export AZ_RC_REGISTRY=
export AZ_RC_SECRET_PASS_KEY=



echo "---------------------------------------"
echo "---------------------------------------"
echo "Clear Containers..."
docker rm -vf $(docker ps -a -q)
echo "---------------------------------------"
echo "---------------------------------------"
echo "Clear Images..."
docker rmi $(docker images -a -q)
echo "---------------------------------------"
echo "---------------------------------------"
echo "Azure Arc Login..."

docker login ${AZ_RC_NAME_ID} -u ${AZ_RC_ACCESS_USER_ID} -p ${AZ_RC_SECRET_PASS_KEY}

docker pull ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ONBOARDING_API_ALIAS}:${IMAGE_TAG}
echo "Pulled ... ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ONBOARDING_API_ALIAS}:${IMAGE_TAG}"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------
echo "Onboarding"
echo "---------------------------------------"
echo "---------------------------------------"

if [ "$(docker ps -q -f name=sw_onboarding_api_${ENV_ALIAS})" ]; then
    echo "Stopping...onboarding"
    docker stop sw_onboarding_api_${ENV_ALIAS} 
    if [ "$(docker ps -aq -f status=exited -f name=sw_onboarding_api_${ENV_ALIAS} )" ]; then
        # cleanup
        echo "Removing...onboarding"
        docker rm -f sw_onboarding_api_${ENV_ALIAS} 
    fi
fi
    # run the container
    echo "Creating and starting...onboarding"
    docker run \
    --name sw_onboarding_api_${ENV_ALIAS} \
    -p 6700:5700 \
    -d \
    ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ONBOARDING_API_ALIAS}:${IMAGE_TAG}

    docker logs sw_onboarding_api_${ENV_ALIAS}  

    docker ps -a -f name=sw_onboarding_api_${ENV_ALIAS}

    docker logs sw_onboarding_api_${ENV_ALIAS}  

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------


docker pull ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${GATEWAY_API_ALIAS}:${IMAGE_TAG}
echo "Pulled ... ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${GATEWAY_API_ALIAS}:${IMAGE_TAG}"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------
echo "AG"
echo "---------------------------------------"
echo "---------------------------------------"

if [ "$(docker ps -q -f name=sw_gw_api_${ENV_ALIAS})" ]; then
    echo "Stopping...gw"
    docker stop sw_gw_api_${ENV_ALIAS} 
    if [ "$(docker ps -aq -f status=exited -f name=sw_gw_api_${ENV_ALIAS} )" ]; then
        # cleanup
        echo "Removing...gw"
        docker rm -f sw_gw_api_${ENV_ALIAS} 
    fi
fi
    # run the container
    echo "Creating and starting...gw"
    docker run \
    --name sw_gw_api_${ENV_ALIAS} \
    -p 8080:5000 \
    -d \
    ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${GATEWAY_API_ALIAS}:${IMAGE_TAG}

    docker logs sw_gw_api_${ENV_ALIAS}  

    docker ps -a -f name=sw_gw_api_${ENV_ALIAS}

    docker logs sw_gw_api_${ENV_ALIAS}  

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------

echo "---------------------------------------"
echo "---------------------------------------"


docker pull ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${INTERPROCESS_API_ALIAS}:${IMAGE_TAG}
echo "Pulled ... ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${INTERPROCESS_API_ALIAS}:${IMAGE_TAG}"


#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------
echo "Interprocess"
echo "---------------------------------------"
echo "---------------------------------------"

if [ "$(docker ps -q -f name=sw_interprocess_api_${ENV_ALIAS})" ]; then
    echo "Stopping...Interprocess"
    docker stop sw_interprocess_api_${ENV_ALIAS} 
    if [ "$(docker ps -aq -f status=exited -f name=sw_interprocess_api_${ENV_ALIAS} )" ]; then
        # cleanup
        echo "Removing...Interprocess"
        docker rm -f sw_interprocess_api_${ENV_ALIAS} 
    fi
fi
    # run the container
    echo "Creating and starting...Interprocess"
    docker run \
    --name sw_interprocess_api_${ENV_ALIAS} \
    -p 6500:5500 \
    -d \
    ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${INTERPROCESS_API_ALIAS}:${IMAGE_TAG}

    docker logs sw_interprocess_api_${ENV_ALIAS}  

    docker ps -a -f name=sw_interprocess_api_${ENV_ALIAS}

    docker logs sw_interprocess_api_${ENV_ALIAS}  

echo "---------------------------------------"
echo "---------------------------------------"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------

docker pull ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${AUTH_API_ALIAS}
echo "Pulled ... ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${AUTH_API_ALIAS}"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------
echo "Auth"
echo "---------------------------------------"
echo "---------------------------------------"

if [ "$(docker ps -q -f name=sw_auth_api_${ENV_ALIAS})" ]; then
    echo "Stopping...Auth"
    docker stop sw_auth_api_${ENV_ALIAS} 
    if [ "$(docker ps -aq -f status=exited -f name=sw_auth_api_${ENV_ALIAS} )" ]; then
        # cleanup
        echo "Removing...auth"
        docker rm -f sw_auth_api_${ENV_ALIAS} 
    fi
fi
    # run the container
    echo "Creating and starting...auth"
    docker run \
    --name sw_auth_api_${ENV_ALIAS} \
    -p 6200:5200 \
    -d \
    ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${AUTH_API_ALIAS}:${IMAGE_TAG}

    docker logs sw_auth_api_${ENV_ALIAS}  

    docker ps -a -f name=sw_auth_api_${ENV_ALIAS}

    docker logs sw_auth_api_${ENV_ALIAS}  

echo "---------------------------------------"
echo "---------------------------------------"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------

docker pull ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ADMIN_API_ALIAS}
echo "Pulled ... ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ADMIN_API_ALIAS}"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------
echo "Admin"
echo "---------------------------------------"
echo "---------------------------------------"

if [ "$(docker ps -q -f name=sw_admin_api_${ENV_ALIAS})" ]; then
    echo "Stopping...admin"
    docker stop sw_admin_api_${ENV_ALIAS} 
    if [ "$(docker ps -aq -f status=exited -f name=sw_admin_api_${ENV_ALIAS} )" ]; then
        # cleanup
        echo "Removing...admin"
        docker rm -f sw_admin_api_${ENV_ALIAS} 
    fi
fi
    # run the container
    echo "Creating and starting...admin"
    docker run \
    --name sw_admin_api_${ENV_ALIAS} \
    -p 6400:5400 \
    -d \
    ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ADMIN_API_ALIAS}:${IMAGE_TAG}

    docker logs sw_admin_api_${ENV_ALIAS}  

    docker ps -a -f name=sw_admin_api_${ENV_ALIAS}

    docker logs sw_admin_api_${ENV_ALIAS}  

echo "---------------------------------------"
echo "---------------------------------------"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------

docker pull ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ASSET_API_ALIAS}
echo "Pulled ... ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ASSET_API_ALIAS}"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------
echo "Asset"
echo "---------------------------------------"
echo "---------------------------------------"

if [ "$(docker ps -q -f name=sw_asset_api_${ENV_ALIAS})" ]; then
    echo "Stopping...asset"
    docker stop sw_asset_api_${ENV_ALIAS} 
    if [ "$(docker ps -aq -f status=exited -f name=sw_asset_api_${ENV_ALIAS} )" ]; then
        # cleanup
        echo "Removing...asset"
        docker rm -f sw_asset_api_${ENV_ALIAS} 
    fi
fi
    # run the container
    echo "Creating and starting...asset"
    docker run \
    --name sw_asset_api_${ENV_ALIAS} \
    -p 6300:5300 \
    -d \
    ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ASSET_API_ALIAS}:${IMAGE_TAG}

    docker logs sw_asset_api_${ENV_ALIAS}  

    docker ps -a -f name=sw_asset_api_${ENV_ALIAS}

    docker logs sw_asset_api_${ENV_ALIAS}  

echo "---------------------------------------"
echo "---------------------------------------"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------

docker pull ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ROUTING_API_ALIAS}
echo "Pulled ... ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ROUTING_API_ALIAS}"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------
echo "Routing"
echo "---------------------------------------"
echo "---------------------------------------"

if [ "$(docker ps -q -f name=sw_routing_api_${ENV_ALIAS})" ]; then
    echo "Stopping...routing"
    docker stop sw_routing_api_${ENV_ALIAS} 
    if [ "$(docker ps -aq -f status=exited -f name=sw_routing_api_${ENV_ALIAS} )" ]; then
        # cleanup
        echo "Removing...routing"
        docker rm -f sw_routing_api_${ENV_ALIAS} 
    fi
fi
    # run the container
    echo "Creating and starting...routing"
    docker run \
    --name sw_routing_api_${ENV_ALIAS} \
    -p 6600:5600 \
    -d \
    ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${ROUTING_API_ALIAS}:${IMAGE_TAG}

    docker logs sw_routing_api_${ENV_ALIAS}  

    docker ps -a -f name=sw_routing_api_${ENV_ALIAS}

    docker logs sw_routing_api_${ENV_ALIAS}  

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------

echo "---------------------------------------"
echo "---------------------------------------"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------

docker pull ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${UI_ALIAS}
echo "Pulled ... ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${UI_ALIAS}"

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------
echo "UI"
echo "---------------------------------------"
echo "---------------------------------------"

if [ "$(docker ps -q -f name=sw_ui_${ENV_ALIAS})" ]; then
    echo "Stopping...ui"
    docker stop sw_ui_${ENV_ALIAS} 
    if [ "$(docker ps -aq -f status=exited -f name=sw_ui_${ENV_ALIAS} )" ]; then
        # cleanup
        echo "Removing...ui"
        docker rm -f sw_ui_${ENV_ALIAS} 
    fi
fi
    # run the container
    echo "Creating and starting...ui"
    docker run \
    --name sw_ui_${ENV_ALIAS} \
    -p 80:5100 \
    -d \
    ${AZ_RC_NAME_ID}/${AZ_RC_REGISTRY}/${UI_ALIAS}:${IMAGE_TAG}

    docker logs sw_ui_${ENV_ALIAS}  

    docker ps -a -f name=sw_ui_${ENV_ALIAS}

    docker logs sw_ui_${ENV_ALIAS}  

#---------------------------------------------------------------------------------------------------------
#---------------------------------------------------------------------------------------------------------



echo "---------------------------------------"
echo "---------------------------------------"
echo "Print All..."

docker ps -a