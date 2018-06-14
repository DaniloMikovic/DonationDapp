pragma solidity ^0.4.16;

interface token {
    function transfer(address receiver, uint amount) external;
}

contract Donation {
    address public owner;
    address public beneficiary;
    uint public donationGoal;
	
    uint public amountRaised;
    uint public startTime;             
    uint public endTime;
	
    uint public price;
    uint public totalNumOfDonators = 0;
    token public tokenReward;
	
    mapping(address => uint256) public balanceOf;
    mapping(uint => address) public donators;
	
    bool donationGoalReached = false;
    bool donationClosed = false;

    event GoalReached(address recipient, uint totalAmountRaised);
    event FundTransfer(address donator, uint amount, bool isContribution);

    function Donation(
        address ifSuccessfulSendTo,
        uint donationGoalInEthers,
        uint etherPriceOfEachToken,
		address addressOfTokenUsedAsReward,
        uint startTimeAsTimestamp,
        uint endTimeAsTimestamp
    ) public {
        require(startTimeAsTimestamp >= now);
        require(endTimeAsTimestamp > startTimeAsTimestamp);

        owner = msg.sender;
        beneficiary = ifSuccessfulSendTo;
        donationGoal = donationGoalInEthers * 1 ether;
        price = etherPriceOfEachToken * 1 ether;
        tokenReward = token(addressOfTokenUsedAsReward);
        startTime = startTimeAsTimestamp;
        endTime = endTimeAsTimestamp;
    }
	
    function () public payable {
        require(!donationClosed);
        require (validDonation());

        uint amount = msg.value;
        balanceOf[msg.sender] += amount;

        amountRaised += amount;

        if (!donatorExists(msg.sender)) {
            // add donator to 'donators' mapping
            donators[totalNumOfDonators] = msg.sender;
            totalNumOfDonators++;
        }          

        tokenReward.transfer(msg.sender, amount / price * 1 ether);
      
        emit FundTransfer(msg.sender, amount, true);

        if (amountRaised >= donationGoal) {
            donationGoalReached = true;

            emit GoalReached(beneficiary, amountRaised);

            payoutBeneficiary();
        }
    }
	
    function safeWithdrawal() afterDeadline ownerOrBeneficiary public {
        
            payoutBeneficiary();
    }

    function donatorExists(address donatorAddress) private view returns (bool) {
        for (uint i = 0; i < totalNumOfDonators; i++) {
            if (donators[i] == donatorAddress) {
                return true;
            }
        }
        return false;
    }

    function payoutBeneficiary() private {
        beneficiary.transfer(amountRaised);
        donationClosed = true;
		
		emit FundTransfer(beneficiary, amountRaised, false);
    }

    function validDonation() private constant returns (bool) {
        bool withinPeriod = now >= startTime && now <= endTime;
        bool nonZeroPurchase = msg.value != 0;
        return withinPeriod && nonZeroPurchase;
    }
	
	modifier afterDeadline() { if (now >= endTime) _; }
	
	modifier ownerOrBeneficiary() {
        require(msg.sender == owner || msg.sender == beneficiary);
        _;
    }
}