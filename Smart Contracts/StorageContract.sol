pragma solidity ^0.4.16;
contract CampaignsStorage {
    address public owner;
    uint[] indexes;
    mapping (uint => string) campaigns;

    function addCampaign(string campaingData) public onlyOwner{
        campaigns[indexes.length] = campaingData;
        indexes.push(indexes.length);
    }
    
    function getCampaign(uint index) public view returns(string){
        return campaigns[index];
    }
    function CampaignsStorage()public{
        owner = msg.sender;
    }
    
    modifier onlyOwner() {
        require(msg.sender == owner);
        _;
    }
}