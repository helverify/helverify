#!/bin/bash

bcscripts=blockchain/*.sh

cp $bcscripts ../consensus-node/backend/Helverify.ConsensusNode.Backend/scripts
cp $bcscripts ../voting-authority/backend/Helverify.VotingAuthority.Backend/scripts